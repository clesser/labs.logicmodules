import fs from "node:fs";
import path from "node:path";

const docRoot = process.cwd();
const sourceRoot = path.resolve(docRoot, "content");
const targetRoot = path.resolve(docRoot, "public", "content");

function copyDirectoryRecursive(sourceDir, targetDir) {
  const entries = fs.readdirSync(sourceDir, { withFileTypes: true });

  for (const entry of entries) {
	const sourcePath = path.join(sourceDir, entry.name);
	const targetPath = path.join(targetDir, entry.name);

	if (entry.isDirectory()) {
	  copyDirectoryRecursive(sourcePath, targetPath);
	  continue;
	}

	if (!entry.isFile()) {
	  continue;
	}

	if (!/\.(png|jpg|jpeg|gif|svg)$/i.test(entry.name)) {
	  continue;
	}

	fs.mkdirSync(path.dirname(targetPath), { recursive: true });
	fs.copyFileSync(sourcePath, targetPath);
  }
}

if (!fs.existsSync(sourceRoot)) {
  throw new Error(`Content source folder not found: ${sourceRoot}`);
}

if (fs.existsSync(targetRoot)) {
  fs.rmSync(targetRoot, { recursive: true, force: true });
}

fs.mkdirSync(targetRoot, { recursive: true });
copyDirectoryRecursive(sourceRoot, targetRoot);

console.log("Synchronized content images to public/content.");
