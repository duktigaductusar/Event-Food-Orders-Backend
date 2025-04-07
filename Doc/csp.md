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

## Best Practices

- Use `script-src 'self'` to block external or inline scripts
- Avoid `'unsafe-inline'` for scripts — **only allow for styles** if needed
- Use `report-uri` or `report-to` to log violations (for monitoring)
- Don’t use `*` (wildcard) in production CSP
- Prefer setting CSP via HTTP headers in production for better protection
