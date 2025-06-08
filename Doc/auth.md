# Auth


Using Microsoft Entra ID (formerly Azure AD) as an authentication provider does not **automatically** enforce any access control unless you explicitly configure it.

Let's break this down so it’s crystal clear:

---

## ✅ What's happening in your current setup?

You **do configure authentication** in your app:

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://login.microsoftonline.com/{configuration["AzureAd:TenantId"]}/v2.0";
        options.Audience = configuration["AzureAd:ClientId"];
    });
```

This makes the app capable of **validating JWT tokens issued by Microsoft Entra**. So it will *know* if a user is authenticated. However...

---

## ❌ What's missing?

You're **not enforcing access control** anywhere in your code. Specifically:

- You're not using `[Authorize]` attributes on any controllers or actions.
- This means all endpoints are **public**, and any user — authenticated or not — can access them.

So while Entra issues the token and your app *could* validate it, you **never require it** to.

---

## ✅ What should be done?

To enforce that a user must be authenticated, use:

```csharp
[Authorize]
public class OrdersController : ControllerBase
{
    ...
}
```

Or for individual actions:

```csharp
public class OrdersController : ControllerBase
{
    [Authorize]
    public IActionResult GetOrders() { ... }

    [AllowAnonymous]
    public IActionResult Ping() => Ok("Healthy");
}
```

You can also enforce policy-based or role-based access like this:

```csharp
[Authorize(Roles = "Admin")]
```

Or based on scopes (from Entra):

```csharp
[Authorize(Policy = "RequireScope:access_as_user")]
```

To enable scope-based auth, you’d need to define policies in `ConfigureServices`:

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("RequireScope:access_as_user", policy =>
    {
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == "scp" && c.Value.Split(' ').Contains("access_as_user")));
    });
});
```

This aligns with Microsoft docs on API access protection:
📖 [Protect web APIs: App roles & scopes](https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-protected-web-api-overview)

---

## 🧠 But what about HttpClient and service classes?

Those service classes likely **call Entra endpoints** (like Graph API) **as the app itself or on behalf of a user** using delegated or client credentials flows. These classes must handle acquiring and attaching access tokens.

But that's *outbound*. It doesn't protect your own endpoints. That still requires `[Authorize]`.

---

## ✅ Recommended steps for you

1. **Audit your controllers** – Decide which ones should require authentication.
2. **Use `[Authorize]`** on those controllers or actions.
3. Optionally, define **role-based or scope-based policies** if needed.
4. Use `[AllowAnonymous]` only on safe endpoints (e.g., health checks).

---


## Service class call Graph 

`UserService` class confirms an important architectural point: **your app authenticates clients via Microsoft Entra ID and then makes outbound calls to Microsoft Graph API using a separate application identity** (likely via **client credentials flow**). This is a *common and valid* architecture, but your concern about the missing `[Authorize]` attribute is still highly relevant.

Let me explain this clearly and provide recommendations.

---

## ✅ What your service class does

Your `UserService` is:
- Using an injected `IGraphTokenService` to acquire an **access token for Microsoft Graph**.
- Making **authenticated outbound requests** to Graph (e.g., `https://graph.microsoft.com/v1.0/users`).
- Acting as a **server-side integration**, not relying on the authenticated user’s identity.

This means:
- You probably registered this app in Azure Entra with **Application permissions** (not Delegated), which gives it backend access to Graph API.
- These permissions are granted via Azure Portal under **API permissions > Application permissions**.

---

## ✅ What this **does not** do

This **does not protect your own API endpoints**. You still need:

- `[Authorize]` attributes to control **who** can access your own API.
- Policies or roles to enforce **what** different users can do.

Even though you have this in `Program.cs`:

```csharp
builder.Services.AddAuthentication(...);
```

You **don’t require authentication** unless you annotate your controller or globally enforce it.

---

## ❌ Risk of not using `[Authorize]`

If none of your controllers use `[Authorize]`, then:

- All endpoints are **public** (open to unauthenticated users).
- Anyone can hit your endpoints, even unauthenticated requests.
- Microsoft Entra is **configured**, but not **enforced**.

---

## ✅ Best practices moving forward

### 1. Add `[Authorize]` to controllers

Apply it to secure your endpoints:

```csharp
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    ...
}
```

You can also use `[Authorize]` at the action level.

---

### 2. Configure role-based or scope-based access (optional, recommended)

If you want to secure endpoints **based on user roles or scopes**, do this:

#### 2.1 Configure policies in `ConfigureAuths`

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireClaim("roles", "Admin"));

    options.AddPolicy("RequireScope:User.Read", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == "scp" && c.Value.Split(' ').Contains("User.Read"))));
});
```

#### 2.2 Then protect actions like:

```csharp
[Authorize(Policy = "RequireAdminRole")]
public IActionResult AdminOnlyAction() { ... }

[Authorize(Policy = "RequireScope:User.Read")]
public IActionResult ScopedAccessAction() { ... }
```

You can inspect the JWT at [jwt.ms](https://jwt.ms/) to see available claims like `roles` or `scp`.

---

### 3. Use `[AllowAnonymous]` sparingly

Only allow anonymous access to safe endpoints like health checks or public info:

```csharp
[AllowAnonymous]
[HttpGet("ping")]
public IActionResult Ping() => Ok("Alive");
```

---

### 4. (Optional) Global Authorization

If your whole API should be protected by default, set a **global authorization policy**:

```csharp
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
```

Then only use `[AllowAnonymous]` where needed.

---

## 🔒 Summary

| **Concern** | **Status** |
|-------------|------------|
| Entra Auth configured | ✅ Yes |
| Access token for Graph API | ✅ Yes |
| Protection of API endpoints | ❌ No (until you add `[Authorize]`) |
| Risk of public access | ⚠️ High unless mitigated |
| Recommendation | ✅ Use `[Authorize]` and optionally policies |

---

### 📚 References

- [Microsoft Docs – Call Microsoft Graph with .NET](https://learn.microsoft.com/en-us/graph/sdks/choose-authentication-providers?tabs=CS)
- [Microsoft Docs – Role-based access control with Entra](https://learn.microsoft.com/en-us/azure/active-directory/develop/howto-add-app-roles-in-azure-ad-apps)
