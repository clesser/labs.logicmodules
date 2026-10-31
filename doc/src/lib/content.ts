import { contentLocaleText } from "./i18n";
import type { Locale, PageData, PageLevel } from "./types";

type MarkdownModule = string | { default: string };

const markdownModules = import.meta.glob("../../content/**/*.md", {
  eager: true,
  query: "?raw",
  import: "default"
}) as Record<string, MarkdownModule>;

function extractRawMarkdown(moduleValue: MarkdownModule): string {
  if (typeof moduleValue === "string") {
	return moduleValue;
  }

  if (moduleValue && typeof moduleValue.default === "string") {
	return moduleValue.default;
  }

  return "";
}

function parseFrontmatter(markdown: string): {
  title: string | null;
  pageLevel?: PageLevel;
  moduleLogo?: string;
  headingTitle?: string;
  content: string;
} {
  const clean = markdown.replace(/^\uFEFF/, "");
  const match = clean.match(/^---\r?\n([\s\S]*?)\r?\n---\r?\n?/);

  if (!match) {
	return { title: null, content: clean };
  }

  const frontmatter = match[1].split(/\r?\n/);
  const content = clean.slice(match[0].length);
  let title: string | null = null;
  let pageLevel: PageLevel | undefined;
  let moduleLogo: string | undefined;
  let headingTitle: string | undefined;

  for (const line of frontmatter) {
	const separator = line.indexOf(":");

	if (separator < 0) {
	  continue;
	}

	const key = line.slice(0, separator).trim();
	const value = line.slice(separator + 1).trim();

	if (key === "title" && value.length > 0) {
	  title = value;
	  continue;
	}

	if (key === "pageLevel") {
	  const normalized = value as PageLevel;
	  if (normalized === "root" || normalized === "package" || normalized === "module" || normalized === "other") {
		pageLevel = normalized;
	  }
	  continue;
	}

	if (key === "moduleLogo" && value.length > 0) {
	  moduleLogo = value;
	  continue;
	}

	if (key === "headingTitle" && value.length > 0) {
	  headingTitle = value;
	}
  }

  return {
	title,
	pageLevel,
	moduleLogo,
	headingTitle,
	content
  };
}

function parseRelativeContentPath(relativePath: string): { locale: Locale; slug: string } | null {
  const match = relativePath.match(/^(.*)\.(de|en)\.md$/);

  if (!match) {
	return null;
  }

  const slugPath = match[1];
  const locale = match[2] as Locale;
  const slug = slugPath === "index" ? "index" : slugPath.replace(/\/index$/, "");

  return { locale, slug };
}

function parseAttributes(raw: string): Record<string, string> {
  const attributes: Record<string, string> = {};
  const regex = /(\w+)="([^"]*)"/g;
  let match: RegExpExecArray | null;

  do {
	match = regex.exec(raw);

	if (match) {
	  attributes[match[1]] = match[2];
	}
  } while (match);

  return attributes;
}

function slugifyHeading(value: string): string {
  return value
	.toLowerCase()
	.trim()
	.replace(/\s+/g, "-")
	.replace(/[^a-z0-9\-äöüß]/g, "");
}

function toLevelAnchor(level: number, heading: string): string {
  const slug = slugifyHeading(heading);
  const stem = slug.length > 0 ? slug : "section";
  return `${level}-${stem}`;
}

function parseHeadingLevel(rawLevel: string | undefined): number {
  const parsed = Number.parseInt(rawLevel ?? "2", 10);

  if (Number.isNaN(parsed) || parsed < 1 || parsed > 6) {
	return 2;
  }

  return parsed;
}

function normalizeAnchorId(anchor: string, level: number): string {
  const clean = anchor.replace(/^#/, "").trim();

  if (clean.length === 0) {
	return toLevelAnchor(level, "section");
  }

  if (/^\d+-/.test(clean)) {
	return clean;
  }

  return toLevelAnchor(level, clean);
}

function buildInternalAnchorHref(path: string, level: number, sectionOrAnchor?: string): string {
  const hashIndex = path.indexOf("#");
  const basePath = hashIndex >= 0 ? path.slice(0, hashIndex) : path;
  const hashInPath = hashIndex >= 0 ? path.slice(hashIndex + 1) : "";
  const anchorSource = (sectionOrAnchor ?? hashInPath).trim();

  if (anchorSource.length === 0) {
	return basePath;
  }

  const anchor = normalizeAnchorId(anchorSource, level);
  return `${basePath}#${anchor}`;
}

function renderShortcode(name: string, attributes: Record<string, string>, locale: Locale): string {
  const localeText = contentLocaleText[locale];

  if (name === "mitLicense") {
	const fallbackYears = `2022-${new Date().getFullYear()}`;
	const yearsAttribute = attributes.years?.trim();
	const years = yearsAttribute && yearsAttribute.length > 0
	  ? yearsAttribute
	  : fallbackYears;
	return localeText.mitLicenseText(years);
  }

  if (name === "downloadLinks") {
	const knx = attributes.knx ?? "";
	const appstore = attributes.appstore ?? "";
	const github = attributes.github ?? "https://github.com/clesser/labs.logicmodules";
	return localeText.downloadLinksText(knx, appstore, github);
  }

  if (name === "moduleLicenseLink") {
	const level = parseHeadingLevel(attributes.level);
	const path = attributes.path ?? (locale === "de" ? "/de" : "/en");
	const target = buildInternalAnchorHref(path, level, attributes.section);
	return localeText.moduleLicenseLinkText.replace("{target}", target);
  }

  if (name === "sectionLink") {
	const level = parseHeadingLevel(attributes.level);
	const path = attributes.path ?? "";
	if (path.trim().length === 0) {
	  return "";
	}
	const label = (attributes.label ?? attributes.section ?? attributes.anchor ?? localeText.sectionLinkDefaultLabel).trim();
	const target = buildInternalAnchorHref(path, level, attributes.anchor ?? attributes.section);
	return `[${label}](${target})`;
  }

  if (name === "githubIssue") {
	const number = (attributes.number ?? "").trim();
	if (!number) {
	  return "";
	}
	const repo = (attributes.repo ?? "clesser/labs.logicmodules").trim();
	const label = (attributes.label ?? `#${number}`).trim();
	return `[${label}](https://github.com/${repo}/issues/${number})`;
  }

  if (name === "br") {
	return "<br />";
  }

  return "";
}

function expandShortcodes(markdown: string, locale: Locale): string {
  return markdown.replace(/\{\{(\w+)([^}]*)\}\}/g, (_full, name: string, attributesRaw: string) => {
	const attributes = parseAttributes(attributesRaw);
	return renderShortcode(name, attributes, locale);
  });
}

function expandDirectives(markdown: string, locale: Locale): string {
  return markdown.replace(/:::warning\n([\s\S]*?)\n:::/g, (_full, content: string) => {
	const title = contentLocaleText[locale].warningTitle;
	return `**${title}:** ${content.trim()}`;
  });
}

const pages = new Map<string, PageData>();

for (const [modulePath, moduleValue] of Object.entries(markdownModules)) {
  const rawMarkdown = extractRawMarkdown(moduleValue);
  const normalizedPath = modulePath.split("\\").join("/");
  const relative = normalizedPath.split("/content/")[1];

  if (!relative) {
	continue;
  }

  const parsedPath = parseRelativeContentPath(relative);

  if (!parsedPath) {
	continue;
  }

  const { locale, slug } = parsedPath;
  const parsed = parseFrontmatter(rawMarkdown);
  const body = expandDirectives(expandShortcodes(parsed.content, locale), locale);

  const slugParts = slug.split("/");
  const localeText = contentLocaleText[locale];
  const fallbackTitle = slugParts.length > 0
	? slugParts[slugParts.length - 1]
	: localeText.fallbackTitle;
  const title = parsed.title && parsed.title.length > 0
	? parsed.title
	: slug === "index"
	  ? localeText.rootTitle
	  : fallbackTitle;

  pages.set(`${locale}:${slug}`, {
	locale,
	slug,
	title,
	body,
	pageLevel: parsed.pageLevel,
	moduleLogo: parsed.moduleLogo,
	headingTitle: parsed.headingTitle
  });
}

export function getPage(locale: Locale, slug: string): PageData | null {
  return pages.get(`${locale}:${slug}`) ?? null;
}

export function normalizeSlug(input: string | undefined): string {
  if (!input || input.trim().length === 0) {
	return "index";
  }

  const clean = input.replace(/^\/+|\/+$/g, "");
  return clean.length === 0 ? "index" : clean;
}
