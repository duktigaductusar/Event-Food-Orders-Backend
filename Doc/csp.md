# Content Security Policy (CSP)

## What?

**CSP (Content Security Policy)** is a **browser-enforced security feature** that prevents many types of **code injection attacks**, including:

- **Cross-Site Scripting (XSS)**
- **Malicious script injection**
- **Unauthorized third-party resources**

> Think of CSP as a **whitelist**: it tells the browser which sources are *allowed* to load content like JavaScript, styles, images, etc.

---

## 🎯 Why use CSP?

Even if your app is secured with HTTPS and Angular’s sanitization:
- A user might be tricked into loading a malicious script
- A browser extension might try to run unauthorized code
- An XSS vulnerability might be exploited via `<img onerror=...>`, `innerHTML`, or `eval()`

**CSP stops these by default** — even if the browser receives the malicious code, **CSP tells it not to run it.**

---

## Basic Example of CSP with `<meta>` tag

Here’s a simple and effective CSP using a `<meta>` tag in your HTML:

```html
<meta http-equiv="Content-Security-Policy" content="
  default-src 'self';
  script-src 'self';
  style-src 'self' 'unsafe-inline';
  img-src 'self' data:;
  connect-src 'self' https://api.yourdomain.com;
  object-src 'none';
  base-uri 'self';
">
```

### What this means:

| Directive         | Description                                                                 |
|-------------------|-----------------------------------------------------------------------------|
| `default-src 'self'`    | Allow everything (scripts, images, styles) **only** from the same origin |
| `script-src 'self'`     | Allow scripts only from the current domain — **blocks inline JS**       |
| `style-src 'self' 'unsafe-inline'` | Allow styles from self + inline styles (needed for Angular)   |
| `img-src 'self' data:`  | Allow images from same origin and `data:` URIs (e.g., base64 images)     |
| `connect-src 'self' https://api.yourdomain.com` | Allow XHR, fetch, WebSockets to specific domains     |
| `object-src 'none'`     | Disallow all Flash or plugin content                                  |
| `base-uri 'self'`       | Prevent attackers from modifying the `<base>` tag                     |

---

## How to test this

1. Add this tag into your app's `index.html`, **just inside `<head>`**:
   ```html
   <head>
     ...
     <meta http-equiv="Content-Security-Policy" content="...">
   </head>
   ```

2. Try to inject a malicious image tag:
   ```html
   <img src="x" onerror="alert('XSS')">
   ```

3. Browser will **block** the alert and log a **CSP violation** in the console.

---

## Example CSP in an Angular App

If you want a CSP that’s realistic for Angular:

```html
<meta http-equiv="Content-Security-Policy" content="
  default-src 'self';
  script-src 'self';
  style-src 'self' 'unsafe-inline';
  img-src 'self' data:;
  font-src 'self';
  connect-src 'self' https://login.microsoftonline.com https://graph.microsoft.com;
  object-src 'none';
  base-uri 'self';
">
```

> Adjust the `connect-src` and `script-src` if you're using MSAL, Graph API, or external fonts.

---

## Use CSP in NGINX insteade of HTML Meta Tag


## You *can* use `<meta http-equiv="Content-Security-Policy">`

Example inside `index.html`:

```html
<head>
  <meta http-equiv="Content-Security-Policy" content="
    default-src 'self';
    script-src 'self';
    style-src 'self' 'unsafe-inline';
    connect-src 'self' https://api.yourdomain.com https://login.microsoftonline.com https://graph.microsoft.com;
    object-src 'none';
    base-uri 'self';
  ">
</head>
```

This works in **modern browsers** and is **browser-enforced**

---

## But why it’s **not preferred** in production

| Problem | Explanation |
|--------|-------------|
| 🕐 **Too late** | The `<meta>` tag is only parsed *after* the browser starts processing HTML. If any scripts are loaded *before* the tag, CSP may not apply. |
| ✏️ **Tamperable** | An attacker who finds an XSS vector could potentially **inject their own `<meta>` tag** before the existing one (rare, but possible with stored XSS). |
| 🧱 **Can’t protect headers** | You can’t set `report-uri`, `require-trusted-types-for`, or `frame-ancestors` via `<meta>`. These only work via **HTTP headers**. |
| 🔄 **Harder to update** | If your CSP needs to change (e.g., new API endpoints), editing the `index.html` file in CI/CD is more awkward than server-side config. |

---

## Why **HTTP headers** are preferred for CSP

```http
Content-Security-Policy: default-src 'self'; script-src 'self'; ...
```

Benefits:
- ✅ Enforced **before any HTML is parsed**
- ✅ Cannot be bypassed by injected HTML
- ✅ Supports all directives
- ✅ Easy to manage in your **NGINX** config or **ASP.NET middleware**
- ✅ Works with **reporting**, like:
  ```http
  Content-Security-Policy: ...; report-uri https://report-uri.com/your-endpoint
  ```

---

## When to use `<meta>` instead

| Scenario | Use `<meta>` tag? |
|----------|-------------------|
| 🔧 Testing CSP locally or in dev | ✅ Yes — quick and useful |
| 🗂️ Hosting on static CDN with no headers (e.g. GitHub Pages) | ✅ Acceptable fallback |
| 🔐 Production with server control (e.g. NGINX, Azure, Netlify) | ❌ Prefer **HTTP headers** |

---

## Summary

| Method      | Secure? | Supports all directives? | Preferred in prod? |
|-------------|---------|---------------------------|---------------------|
| `<meta http-equiv="Content-Security-Policy">` | ✅ Mostly | ❌ Some features missing | ❌ No |
| `Content-Security-Policy` HTTP header         | ✅✅✅ | ✅ Yes | ✅✅✅ Yes |

---

### Final Recommendation

Since you're serving Angular from **NGINX**:
- Use **HTTP headers in NGINX** to set CSP ✅
- Use the `<meta>` tag only if you're testing or hosting somewhere that doesn’t allow headers (e.g., GitHub Pages)

## Best Practices

- Use `script-src 'self'` to block external or inline scripts
- Avoid `'unsafe-inline'` for scripts — **only allow for styles** if needed
- Use `report-uri` or `report-to` to log violations (for monitoring)
- Don’t use `*` (wildcard) in production CSP
- Prefer setting CSP via HTTP headers in production for better protection
