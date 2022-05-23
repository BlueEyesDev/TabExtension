# TabExtension
A plugin system for loading usercontrols in a tabcontrol for Windows Forms 4.8

For an automatic copy of the library modify the .csproj file of the library to add this line before </Project>

```xml
  <Target Name="BuildCompile">
	<Exec Command='copy "$(TargetPath)" "$(SolutionDir)Exemple\bin\Debug\Tabs\$(TargetName).Tabs"' Condition="Exists('$(SolutionDir)Exemple\bin\Debug\Tabs')"/>
  </Target>
  
  <Target Name="AfterRebuild">
	<Exec Command='copy "$(TargetPath)" "$(SolutionDir)Exemple\bin\Debug\Tabs\$(TargetName).Tabs"' Condition="Exists('$(SolutionDir)Exemple\bin\Debug\Tabs')"/>
  </Target>
  
  <Target Name="AfterBuild">
	<Exec Command='copy "$(TargetPath)" "$(SolutionDir)Exemple\bin\Debug\Tabs\$(TargetName).Tabs"' Condition="Exists('$(SolutionDir)Exemple\bin\Debug\Tabs')"/>
  </Target>
```

and changed "Example" to your application path
