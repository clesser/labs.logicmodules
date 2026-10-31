export type Locale = "de" | "en";

export type PageLevel = "root" | "package" | "module" | "other";

export interface HeadingEntry {
  title: string;
  anchor: string;
}

export interface LanguageSwitchProps {
  locale: Locale;
  slug: string;
}

export interface PageViewProps {
  locale: Locale;
  wildcardPath?: string;
}

export interface LocaleRouteProps {
  locale: Locale;
}

export interface PageData {
  locale: Locale;
  slug: string;
  title: string;
  body: string;
  pageLevel?: PageLevel;
  moduleLogo?: string;
  headingTitle?: string;
}
