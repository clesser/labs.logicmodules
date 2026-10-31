import type { Locale } from "./types";

interface AppLocaleText {
  donateTitle: string;
  headerPackageLabel: string;
  headerHelpLabel: string;
  rootBackLabel: string;
  childBackLabel: string;
  notFoundText: string;
}

interface ContentLocaleText {
  mitLicenseText: (years: string) => string;
  downloadLinksText: (knx: string, appstore: string, github: string) => string;
  moduleLicenseLinkText: string;
  warningTitle: string;
  fallbackTitle: string;
  rootTitle: string;
  sectionLinkDefaultLabel: string;
}

export const appLocaleText: Record<Locale, AppLocaleText> = {
  de: {
	donateTitle: "Spenden",
	headerPackageLabel: "Logikpakete",
	headerHelpLabel: "Hilfe",
	rootBackLabel: "Start",
	childBackLabel: "Zurück",
	notFoundText: "Diese Seite wurde nicht gefunden."
  },
  en: {
	donateTitle: "Donate",
	headerPackageLabel: "Logic Packages",
	headerHelpLabel: "Help",
	rootBackLabel: "Home",
	childBackLabel: "Back",
	notFoundText: "This page was not found."
  }
};

export const contentLocaleText: Record<Locale, ContentLocaleText> = {
  de: {
	mitLicenseText: (years: string) => `Die Logikbausteine ("Software"), Dokumentation und Grafiken in diesem Paket stehen unter der [MIT-Lizenz](https://mit-license.org/) ([Sinngemäße Übersetzung](https://de.wikipedia.org/wiki/MIT-Lizenz)), sofern Einzelkomponenten des Pakets nicht durch andere Lizenzierungen gekennzeichnet sind.\n\nCopyright © ${years} - Christoph Lesser - labs.neleo.com`,
	downloadLinksText: (knx: string, appstore: string, github: string) => `Die aktuelle Version des Logikpakets ist im [KNX User Forum](${knx}) und im [AppStore für den Gira X1](${appstore}) verfügbar. Der Source Code liegt auf [GitHub](${github}).`,
	moduleLicenseLinkText: "- [Lizenzbedingungen für diesen Logikbaustein]({target})",
	warningTitle: "Hinweis",
	fallbackTitle: "Dokumentation",
	rootTitle: "Start",
	sectionLinkDefaultLabel: "Link"
  },
  en: {
	mitLicenseText: (years: string) => `The logic modules ("Software"), documentation, and graphics in this logic module are provided under the [MIT License](https://mit-license.org/), unless individual components are explicitly marked with a different license.\n\nCopyright © ${years} - Christoph Lesser - labs.neleo.com`,
	downloadLinksText: (knx: string, appstore: string, github: string) => `The latest version of this logic module is available in the [KNX User Forum](${knx}) and in the [AppStore for the Gira X1](${appstore}). The source code is available on [GitHub](${github}).`,
	moduleLicenseLinkText: "- [License terms for this logic module]({target})",
	warningTitle: "Note",
	fallbackTitle: "Documentation",
	rootTitle: "Home",
	sectionLinkDefaultLabel: "Link"
  }
};
