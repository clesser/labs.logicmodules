import React from "react";
import ReactMarkdown from "react-markdown";
import { Navigate, Route, Routes, useLocation, useNavigate } from "react-router-dom";
import rehypeAutolinkHeadings from "rehype-autolink-headings";
import rehypeRaw from "rehype-raw";
import rehypeSlug from "rehype-slug";
import remarkGfm from "remark-gfm";
import { getPage, normalizeSlug } from "./lib/content";
import { appLocaleText } from "./lib/i18n";
import type {
  HeadingEntry,
  LanguageSwitchProps,
  Locale,
  LocaleRouteProps,
  PageLevel,
  PageViewProps
} from "./lib/types";

function HashScrollHandler(): React.ReactElement | null {
  const location = useLocation();

  React.useEffect(() => {
    if (!location.hash || location.hash.length <= 1) {
      return;
    }

    let cancelled = false;
    let attempts = 0;
    const maxAttempts = 10;
    const anchorId = decodeURIComponent(location.hash.slice(1));

    const scrollToAnchor = (): void => {
      if (cancelled) {
        return;
      }

      const target = document.getElementById(anchorId);
      if (target) {
        target.scrollIntoView();
        return;
      }

      if (attempts < maxAttempts) {
        attempts += 1;
        window.setTimeout(scrollToAnchor, 50);
      }
    };

    window.setTimeout(scrollToAnchor, 0);

    return () => {
      cancelled = true;
    };
  }, [location.pathname, location.hash]);

  return null;
}

function LanguageSwitch({ locale, slug }: LanguageSwitchProps): React.ReactElement {
  const navigate = useNavigate();

  function navigateTo(nextLocale: Locale): void {
	const basePath = slug === "index" ? "" : `/${slug}`;
	navigate(`/${nextLocale}${basePath}`);
  }

  return (
	<div className="language-switch">
	  <button type="button" onClick={() => navigateTo("de")} disabled={locale === "de"}>DE</button>
	  <button type="button" onClick={() => navigateTo("en")} disabled={locale === "en"}>EN</button>
	</div>
  );
}

function slugifyHeading(value: string): string {
  return value
	.toLowerCase()
	.trim()
	.replace(/\s+/g, "-")
	.replace(/[^a-z0-9\-äöüß]/g, "");
}

function toLevelAnchor(level: number, title: string): string {
  const baseAnchor = slugifyHeading(title);
  const stem = baseAnchor.length > 0 ? baseAnchor : "section";
  return `${level}-${stem}`;
}

function extractH2Headings(markdown: string): HeadingEntry[] {
  const matches = [...markdown.matchAll(/^##\s+(.+)$/gm)];

  return matches
	.map((match) => match[1]
	  .replace(/\[([^\]]+)\]\([^\)]+\)/g, "$1")
	  .replace(/[*_`~]/g, "")
	  .replace(/<[^>]+>/g, "")
	  .trim())
	.filter((value) => value.length > 0)
	.map((title) => ({
	  title,
	  anchor: toLevelAnchor(2, title)
	}));
}

function getNodeText(node: React.ReactNode): string {
  if (typeof node === "string" || typeof node === "number") {
	return String(node);
  }

  if (Array.isArray(node)) {
	return node.map((part) => getNodeText(part)).join("");
  }

  if (React.isValidElement(node)) {
	return getNodeText(node.props.children);
  }

  return "";
}

function getBackPath(locale: Locale, slug: string): string {
  if (slug === "index") {
	return `/${locale}`;
  }

  const segments = slug.split("/").filter((segment) => segment.length > 0);

  if (segments.length <= 1) {
	return `/${locale}`;
  }

  const docIndex = segments.indexOf("doc");

  if (docIndex > 0) {
	return `/${locale}/${segments.slice(0, docIndex).join("/")}`;
  }

  return `/${locale}/${segments.slice(0, -1).join("/")}`;
}

function getHeaderBlackText(locale: Locale): string {
  return appLocaleText[locale].headerPackageLabel;
}

function getPackageSlug(slug: string, pageLevel: PageLevel): string | null {
  if (pageLevel === "package") {
	return slug;
  }

  if (pageLevel === "module") {
	const segments = slug.split("/").filter((segment) => segment.length > 0);
	return segments.length > 0 ? segments[0] : null;
  }

  return null;
}

function isExternalHref(href: string | undefined): boolean {
  if (!href) {
	return false;
  }

  return /^(https?:)?\/\//i.test(href);
}

function PageView({ locale, wildcardPath }: PageViewProps): React.ReactElement {
  const slug = normalizeSlug(wildcardPath);
  const page = getPage(locale, slug);
  const fallback = locale === "en" ? getPage("de", slug) : null;
  const activePage = page ?? fallback;
  const h2Headings = activePage ? extractH2Headings(activePage.body) : [];
  const localeText = appLocaleText[locale];
  const backPath = getBackPath(locale, slug);
  const backLabel = slug === "index" ? localeText.rootBackLabel : localeText.childBackLabel;
  const pageLevel: PageLevel = activePage?.pageLevel ?? "other";
  const packageSlug = getPackageSlug(slug, pageLevel);
  const packagePage = packageSlug
	? (getPage(locale, packageSlug) ?? (locale === "en" ? getPage("de", packageSlug) : null))
	: null;
  const headerBlackText = packagePage?.title ?? getHeaderBlackText(locale);
  const moduleLogo = activePage?.moduleLogo;
  const pageTitle = activePage?.headingTitle ?? (activePage?.title ?? slug);

  return (
	<article id="top">
	  <header>
		<span className="headDonate">
		  <a href={`/${locale}/donate`} title={localeText.donateTitle}></a>
		</span>
		<span className="headLogo"></span>
		<span className="headBlack">{headerBlackText}</span>
		<span className="headGray">{localeText.headerHelpLabel}</span>
		<h1 className={moduleLogo ? "module-heading" : "noicon"}>
		  {moduleLogo ? <img className="module-heading-icon" src={moduleLogo} alt="" aria-hidden="true" /> : null}
		  <span className={moduleLogo ? "module-heading-title" : undefined}>{pageTitle}</span>
		</h1>
		<nav>
		  <a href={backPath}>{backLabel}</a>
		  {h2Headings.map((heading) => (
			<a key={`h2-nav-${heading.anchor}`} href={`#${heading.anchor}`}>{heading.title}</a>
		  ))}
		  <LanguageSwitch locale={locale} slug={slug} />
		</nav>
	  </header>

	  <section>
		{!activePage ? (
		  <>
			<p>{localeText.notFoundText}</p>
		  </>
		) : (
		  <ReactMarkdown
			remarkPlugins={[remarkGfm]}
			rehypePlugins={[rehypeRaw, rehypeSlug, [rehypeAutolinkHeadings, { behavior: "append" }]]}
			components={{
			  a: ({ href, children, title }) => {
				const external = isExternalHref(href);

				return (
				  <a
					href={href}
					title={title}
					target={external ? "_blank" : undefined}
					rel={external ? "noopener noreferrer" : undefined}
				  >
					{children}
				  </a>
				);
			  },
			  h2: ({ children }) => {
				const title = getNodeText(children).trim();
				const id = toLevelAnchor(2, title.length > 0 ? title : "section");

				return (
				  <h2 id={id}>
					{children}
					<a className="heading-top-link" href="#">↑</a>
				  </h2>
				);
			  },
			  h3: ({ children }) => {
				const title = getNodeText(children).trim();
				const id = toLevelAnchor(3, title.length > 0 ? title : "section");

				return (
				  <h3 id={id}>
					{children}
					<a className="heading-top-link" href="#">↑</a>
				  </h3>
				);
			  },
			  h4: ({ children }) => {
				const title = getNodeText(children).trim();
				const id = toLevelAnchor(4, title.length > 0 ? title : "section");

				return (
				  <h4 id={id}>
					{children}
					<a className="heading-top-link" href="#">↑</a>
				  </h4>
				);
			  },
			  h5: ({ children }) => {
				const title = getNodeText(children).trim();
				const id = toLevelAnchor(5, title.length > 0 ? title : "section");

				return (
				  <h5 id={id}>
					{children}
					<a className="heading-top-link" href="#">↑</a>
				  </h5>
				);
			  },
			  h6: ({ children }) => {
				const title = getNodeText(children).trim();
				const id = toLevelAnchor(6, title.length > 0 ? title : "section");

				return (
				  <h6 id={id}>
					{children}
					<a className="heading-top-link" href="#">↑</a>
				  </h6>
				);
			  }
			}}
		  >
			{activePage.body}
		  </ReactMarkdown>
		)}
	  </section>

	  <footer>
		<span className="footerGray">
		  <a href={`/${locale}`}>&copy; Christoph Lesser - labs.neleo.com</a>
		</span>
	  </footer>
	</article>
  );
}

function LocaleRoute({ locale }: LocaleRouteProps): React.ReactElement {
  const location = useLocation();
  const prefix = `/${locale}`;
  const path = location.pathname === prefix
	? "index"
	: location.pathname.replace(new RegExp(`^/${locale}/?`), "");

  return <PageView locale={locale} wildcardPath={path} />;
}

function detectPreferredLocale(): Locale {
  if (typeof navigator === "undefined") {
	return "de";
  }

  const candidates = [
	...(navigator.languages ?? []),
	navigator.language
  ].filter((value): value is string => typeof value === "string" && value.length > 0);

  for (const candidate of candidates) {
	const normalized = candidate.toLowerCase();

	if (normalized.startsWith("en")) {
	  return "en";
	}

	if (normalized.startsWith("de")) {
	  return "de";
	}
  }

  return "de";
}

function RootLocaleRedirect(): React.ReactElement {
  const locale = detectPreferredLocale();
  return <Navigate replace to={`/${locale}`} />;
}

function DefaultLocaleRedirect(): React.ReactElement {
  const location = useLocation();
  const path = location.pathname.startsWith("/") ? location.pathname : `/${location.pathname}`;

  if (path.startsWith("/de") || path.startsWith("/en")) {
	return <Navigate replace to="/de" />;
  }

  return <Navigate replace to={`/de${path}`} />;
}

export function App(): React.ReactElement {
  return (
	<>
	  <HashScrollHandler />
	  <Routes>
		<Route path="/" element={<RootLocaleRedirect />} />
		<Route path="/de" element={<LocaleRoute locale="de" />} />
		<Route path="/de/*" element={<LocaleRoute locale="de" />} />
		<Route path="/en" element={<LocaleRoute locale="en" />} />
		<Route path="/en/*" element={<LocaleRoute locale="en" />} />
		<Route path="*" element={<DefaultLocaleRedirect />} />
	  </Routes>
	</>
  );
}
