# Model Driven Apps Deployment Template - Customization Guide

This document provides step-by-step instructions to customize this template for a new project. The template is designed for Dynamics 365/CDS (Model Driven Apps) deployment with plugins, web resources, and solution packaging.

## Overview

When setting up a new project, you need to replace template placeholders with your project-specific names:
- **TestSolution** → Your Project/Solution Name
- **TestSolutionPlugins** → YourProjectPlugins (namespace and DLL name)
- **PluginAssemblyName** → Your actual plugin assembly name (typically same as TestSolutionPlugins)

---

## Prerequisites

1. Visual Studio 2019 or later
2. Access to Dynamics 365/CDS sandbox environment
3. Git installed and configured
4. Repository created for the new project

---

## Step-by-Step Customization Process

### Step 1: Repository Setup

1. Create a new repository for your project
2. Clone the repository to your local machine
3. Clone/copy this template solution to your new project folder (excluding `.vs` and `.git` folders)

### Step 2: Rename Solution File

1. Rename `TestSolution.sln` to `[YourProjectName].sln`
   - Example: `IRISCore.sln`, `MyApp.sln`
   - **Important:** Keep the `.sln` extension

### Step 3: Replace Solution Name (TestSolution)

**Method:** Use Visual Studio Find and Replace

1. Open the solution in Visual Studio
2. Press `Ctrl+H` to open Find and Replace
3. **Find:** `TestSolution`
4. **Replace with:** `[YourProjectName]` (e.g., `IRISCore`, `MyApp`)
5. **Options:** 
   - ✓ Match Case
   - ✓ Match Whole Word (optional, but recommended)
   - Scope: **Entire Solution**
6. Click **Replace All**
7. **Expected:** ~13-15 replacements

**Files that will be updated:**
- `DeploymentPackage\CdsSolution\Other\Solution.xml` (UniqueName and LocalizedName)
- `DeploymentPackage\SolutionSettings.ps1` (CdsSolutionName)
- `DeploymentPackage\PackageTemplate.cs` (Package name in multiple places)
- `DeploymentPackage\CdsArtifacts.nuspec` (Package ID, description, file paths)
- `DeploymentPackage\solution.mappings.xml` (if contains solution name)
- `Plugins\spkl.json` (solution unique name)
- `WebResources\spkl.json` (solution unique name - if different)

### Step 4: Replace Plugin Namespace (TestSolutionPlugins)

**Method:** Use Visual Studio Find and Replace

1. Press `Ctrl+H` to open Find and Replace again
2. **Find:** `TestSolutionPlugins`
3. **Replace with:** `[YourProjectName]Plugins` (e.g., `IRISCorePlugins`, `MyAppPlugins`)
4. **Options:**
   - ✓ Match Case
   - ✓ Match Whole Word
   - Scope: **Entire Solution**
5. Click **Replace All**
6. **Expected:** ~15-18 replacements

**Files that will be updated:**
- `Plugins\PluginBase.cs` (namespace)
- `Plugins.UnitTest\**\*.cs` (all namespace declarations)
- `DeploymentPackage\solution.mappings.xml` (DLL mapping)

### Step 5: Update Plugin Assembly Name

**Manual Steps Required:**

1. Open `Plugins\Plugins.csproj`
   - Find: `<RootNamespace>PluginAssemblyName</RootNamespace>`
   - Replace with: `<RootNamespace>[YourProjectName]Plugins</RootNamespace>`
   - Find: `<AssemblyName>PluginAssemblyName</AssemblyName>`
   - Replace with: `<AssemblyName>[YourProjectName]Plugins</AssemblyName>`

2. Open `Plugins.UnitTest\Plugins.UnitTest.csproj`
   - Find: `<RootNamespace>PluginAssemblyName.UnitTest</RootNamespace>`
   - Replace with: `<RootNamespace>[YourProjectName]Plugins.UnitTest</RootNamespace>`
   - Find: `<AssemblyName>PluginAssemblyName.UnitTest</AssemblyName>`
   - Replace with: `<AssemblyName>[YourProjectName]Plugins.UnitTest</AssemblyName>`

**Note:** If you want a different assembly name (not `[ProjectName]Plugins`), replace `PluginAssemblyName` throughout the codebase using Find and Replace.

### Step 6: Update Solution XML Configuration

**File:** `DeploymentPackage\CdsSolution\Other\Solution.xml`

1. Update the following elements:
   - `<UniqueName>` - Should match your CDS solution unique name
   - `<LocalizedName description="...">` - Display name of your solution
   - Update version if needed: `<Version>0.0.0.1</Version>`
   - Review and update `<Publisher>` information if required

**Note:** This file is typically updated when you export the solution from CDS, so ensure it matches your actual solution in Dynamics 365.

### Step 7: Update Package Configuration Files

#### 7a. DeploymentPackage\CdsArtifacts.nuspec

Update the following:
```xml
<id>IIC.Cds.TestSolution.Sources</id>
```
Replace `TestSolution` with your project name:
```xml
<id>IIC.Cds.[YourProjectName].Sources</id>
```

Update description:
```xml
<description>CDS dependency artifacts for the [YourProjectName] solution.</description>
```

Update file paths:
```xml
<file src="temp\packed\TestSolution_managed.zip" target="content/CdsDependencies/[YourProjectName]_$version$/[YourProjectName]_managed.zip" />
```

#### 7b. DeploymentPackage\SolutionSettings.ps1

Update:
```powershell
CdsSolutionName = "[YourProjectName]"
```

#### 7c. DeploymentPackage\solution.mappings.xml

Update the DLL mapping to match your plugin assembly name:
```xml
<FileToFile map="PluginAssemblies\**\[YourProjectName]Plugins.dll" to="..\..\Plugins\bin\**\[YourProjectName]Plugins.dll" />
```

#### 7d. DeploymentPackage\PackageTemplate.cs

Update all occurrences of "TestSolution" in:
- `GetNameOfImport()` method return value
- `GetImportPackageDescriptionText` property
- `GetLongNameOfImport` property

### Step 8: Update SPKL Configuration Files

#### 8a. Plugins\spkl.json

Update:
- `"solution": "TestSolution"` → `"solution": "[YourProjectName]"`
- `"solution_uniquename": "TestSolution"` → `"solution_uniquename": "[YourProjectName]"`

Review other settings:
- Update entity list in `earlyboundtypes.entities` if needed
- Update actions list if needed
- Update `classNamespace` if desired (default: `TestPlugin` → `[YourProjectName]Plugin`)

#### 8b. WebResources\spkl.json

Update:
- `"solution": "spkltestsolution"` → `"solution": "[yourprojectname]"` (typically lowercase)
- Ensure it matches your CDS solution unique name (may be case-sensitive)

### Step 9: Update Azure DevOps Pipeline (Optional)

**File:** `azure-pipelines.yml`

Update the following if you have project-specific requirements:
- Pipeline name (currently generic)
- Pool name (line 10) - may need updating for your organization
- NuGet feed references (lines 55, 88) - update feed IDs if different
- Any project-specific build steps

**Note:** Most of this file can remain as-is unless you have organization-specific requirements.

### Step 10: Verify Namespace Consistency

1. Rebuild the solution: `Build → Rebuild Solution`
2. Check for compilation errors related to namespaces
3. Ensure all `using` statements are correct
4. Verify test projects reference the correct plugin namespace

### Step 11: Update Connection Parameters (Deployment)

**File:** `DeploymentPackage\DevOps\CdsConnectionParameters\`

1. Review connection parameter templates
2. Create new connection parameter files for your environments
3. Update `Import.ps1` if you add new connection names:
   ```powershell
   [ValidateSet("MySandbox","MyTest","DummyConnection","YourNewConnection")]
   ```

### Step 12: Update README.md

1. Update project name references in `README.md`
2. Update any project-specific instructions
3. Document your specific deployment process

---

## Summary of Replacements

| Placeholder | Replacement | Count | Critical |
|------------|-------------|-------|----------|
| `TestSolution` | `[YourProjectName]` | ~13-15 | ✓ Yes |
| `TestSolutionPlugins` | `[YourProjectName]Plugins` | ~15-18 | ✓ Yes |
| `PluginAssemblyName` | `[YourProjectName]Plugins` | 4 | ✓ Yes |
| `IIC.Cds.TestSolution` | `IIC.Cds.[YourProjectName]` | 2-3 | ✓ Yes |
| `TestSolution_managed.zip` | `[YourProjectName]_managed.zip` | 2-3 | ✓ Yes |
| Solution file name | Rename `.sln` file | 1 | ✓ Yes |

---

## Verification Checklist

After customization, verify:

- [ ] Solution builds without errors
- [ ] All namespaces are consistent
- [ ] Solution.xml matches your CDS solution
- [ ] spkl.json files have correct solution names
- [ ] Assembly names match in all projects
- [ ] solution.mappings.xml points to correct DLL
- [ ] CdsArtifacts.nuspec has correct package ID
- [ ] PackageTemplate.cs has updated descriptions
- [ ] Unit test project builds and can reference plugins
- [ ] Git commit includes all changes

---

## Post-Setup Tasks

1. **Create CDS Solution:** Create an empty solution in your Dynamics 365 sandbox with the exact name matching your configuration
2. **Export Solution:** Export the empty solution and replace `DeploymentPackage\CdsSolution\Other\Solution.xml` if needed
3. **Test Build:** Build the solution to ensure everything compiles
4. **Test Deployment:** Test the deployment package creation
5. **Update Publisher:** Update publisher information if different from template defaults

---

## Troubleshooting

### Build Errors After Customization

**Issue:** Namespace or assembly name mismatches
- **Solution:** Double-check all replacements were done consistently
- Verify `.csproj` files have correct RootNamespace and AssemblyName

**Issue:** DLL not found during package creation
- **Solution:** Check `solution.mappings.xml` matches actual DLL name in `bin\Release` or `bin\Debug`

### Solution Import Errors

**Issue:** Solution name mismatch
- **Solution:** Ensure `Solution.xml` unique name matches exactly with CDS solution
- Verify `spkl.json` files have correct solution names

### Namespace Errors

**Issue:** Cannot find namespace in unit tests
- **Solution:** Verify plugin assembly name matches namespace
- Rebuild solution after namespace changes
- Check project references are intact

---

## Additional Notes

- **Assembly Signing:** The template uses `plugin.snk` for signing. Ensure this file is present and valid for your organization's requirements
- **Package Dependencies:** Review `CdsArtifacts.nuspec` dependencies section if your solution depends on other packages
- **Web Resources:** Customize `WebResources\spkl.json` based on your web resource deployment needs
- **Early Bound Types:** Update entity list in `spkl.json` to include only entities you actually use

---

## Template Version

This guide is for the Model Driven Apps Deployment Template v1.0

For questions or issues, refer to your project documentation or contact your DevOps team.

