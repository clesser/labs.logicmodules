# [neleo.labs] Gira X1 Logic Modules

## User Documentation
Please see [x1.labs.neleo.com](https://x1.labs.neleo.com) (German only).


## Developer Documentation
There's no official documentation as this is a hobbyist project. Nevertheless, a lot of comments can be found in the source code.
To run logic modules on a Gira X1 an official developer certificate must been obtained (my personal certificate has been excluded from the repo).

To bundle and sign the logic modules, the Gira SDK offers two tools that can configured as Build Events in each logic module's project file.

```
"$(SolutionDir)\LogicNodesSDK\LogicNodeTool.exe" create "$(TargetDir) " "$(SolutionDir)\Signed Releases"
"$(SolutionDir)\LogicNodesSDK\SignLogicNodes.exe" "$(SolutionDir)\Certificates\<name-of-the-certificate-file>.p12" "<password-for-the-certificate>" "$(SolutionDir)\Signed Releases"
```

## Disclaimer
This is a private project and not affiliated with Gira, Velux or Nuki. Please see legal notes in user documentation (German only).
