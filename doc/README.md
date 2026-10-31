# New documentation app

This folder contains the new Markdown-first static documentation app.

## Goals

- Keep content in Markdown files using language suffixes (`*.de.md`, `*.en.md`)
- Keep route paths aligned with current folder names
- Use explicit language prefixes in routes (`/de/...`, `/en/...`)
- Keep static legacy HTML redirects in `src/lib/legacyRoutes.ts` (maintained manually)

## Commands

```bash
npm install
npm run sync:content-images
npm run dev
npm run dev:win
npm run build
```

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

Examples:

- `content/index.de.md` -> `/de`
- `content/logic.bridges.nuki/index.de.md` -> `/de/logic.bridges.nuki`
- `content/logic.bridges.nuki/index.en.md` -> `/en/logic.bridges.nuki`
