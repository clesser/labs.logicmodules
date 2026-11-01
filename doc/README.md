# Neleo Labs - X1 Logic Modules Documentation

This folder contains the new Markdown-first static documentation app.

## Notes

- Use explicit language prefixes in routes (`/de/...`, `/en/...`)
- Static legacy HTML redirects defined in `staticwebapp.config.json` routes (maintained manually)

## Content conventions

- Keep all pages under `content/` with language suffixes:
  - German: `*.de.md`
  - English: `*.en.md`
- Use the same file tree in both locales
- Store package images under `content/<package>/img/...` (next to optional `doc/` folders)
- Image URLs in markdown should use `/content/<package>/img/...`
- `index.<lang>.md` in a folder maps to the folder route with locale prefix
- Optional frontmatter controls page header behavior:
  - `pageLevel`: `root` | `package` | `module` | `other`
  - `headingTitle`: overrides the rendered H1 text
  - `moduleLogo`: icon path for module pages (shown before module title)
- Use shortcode-based internal section links for stable anchors:
  - `{{sectionLink path="/en/logic.timing" section="license" label="License"}}`
  - Anchors are normalized to the generated heading IDs (e.g., `#2-license`)
