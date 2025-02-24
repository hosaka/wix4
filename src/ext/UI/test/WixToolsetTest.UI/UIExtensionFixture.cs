// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

namespace WixToolsetTest.UI
{
    using System.IO;
    using System.Linq;
    using WixInternal.Core.TestPackage;
    using WixInternal.TestSupport;
    using WixToolset.Data.WindowsInstaller;
    using WixToolset.UI;
    using Xunit;

    public class UIExtensionFixture
    {
        [Fact]
        public void CanBuildUsingWixUIAdvanced()
        {
            var folder = TestData.Get(@"TestData", "WixUI_Advanced");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(Build, "Binary", "Dialog", "CustomAction", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:AdvancedWelcomeEulaDlg\t"));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_X86\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:WixSetDefaultPerMachineFolder\t51\tWixPerMachineFolder\t[ProgramFilesFolder][ApplicationFolderName]\t",
                "CustomAction:WixSetDefaultPerUserFolder\t51\tWixPerUserFolder\t[LocalAppDataFolder]Apps\\[ApplicationFolderName]\t",
                "CustomAction:WixSetPerMachineFolder\t51\tAPPLICATIONFOLDER\t[WixPerMachineFolder]\t",
                "CustomAction:WixSetPerUserFolder\t51\tAPPLICATIONFOLDER\t[WixPerUserFolder]\t",
                "CustomAction:WixUIPrintEula_X86\t65\tWixUiCa_X86\tPrintEula\t",
                "CustomAction:WixUIValidatePath_X86\t65\tWixUiCa_X86\tValidatePath\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:AdvancedWelcomeEulaDlg\tPrint\tDoAction\tWixUIPrintEula_X86\t1\t1",
                "ControlEvent:BrowseDlg\tOK\tDoAction\tWixUIValidatePath_X86\tNOT WIXUI_DONTVALIDATEPATH\t1",
                "ControlEvent:InstallDirDlg\tNext\tDoAction\tWixUIValidatePath_X86\tNOT WIXUI_DONTVALIDATEPATH\t2",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).ToArray());
        }

        [Fact]
        public void CanBuildUsingWixUIAdvancedX64()
        {
            var folder = TestData.Get(@"TestData", "WixUI_Advanced");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(BuildX64, "Binary", "Dialog", "CustomAction", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:AdvancedWelcomeEulaDlg\t"));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_X64\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:WixSetDefaultPerMachineFolder\t51\tWixPerMachineFolder\t[ProgramFilesFolder][ApplicationFolderName]\t",
                "CustomAction:WixSetDefaultPerUserFolder\t51\tWixPerUserFolder\t[LocalAppDataFolder]Apps\\[ApplicationFolderName]\t",
                "CustomAction:WixSetPerMachineFolder\t51\tAPPLICATIONFOLDER\t[WixPerMachineFolder]\t",
                "CustomAction:WixSetPerUserFolder\t51\tAPPLICATIONFOLDER\t[WixPerUserFolder]\t",
                "CustomAction:WixUIPrintEula_X64\t65\tWixUiCa_X64\tPrintEula\t",
                "CustomAction:WixUIValidatePath_X64\t65\tWixUiCa_X64\tValidatePath\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:AdvancedWelcomeEulaDlg\tPrint\tDoAction\tWixUIPrintEula_X64\t1\t1",
                "ControlEvent:BrowseDlg\tOK\tDoAction\tWixUIValidatePath_X64\tNOT WIXUI_DONTVALIDATEPATH\t1",
                "ControlEvent:InstallDirDlg\tNext\tDoAction\tWixUIValidatePath_X64\tNOT WIXUI_DONTVALIDATEPATH\t2",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).ToArray());
        }

        [Fact]
        public void CanBuildUsingWixUIAdvancedARM64()
        {
            var folder = TestData.Get(@"TestData", "WixUI_Advanced");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(BuildARM64, "Binary", "Dialog", "CustomAction", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:AdvancedWelcomeEulaDlg\t"));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_A64\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:WixSetDefaultPerMachineFolder\t51\tWixPerMachineFolder\t[ProgramFilesFolder][ApplicationFolderName]\t",
                "CustomAction:WixSetDefaultPerUserFolder\t51\tWixPerUserFolder\t[LocalAppDataFolder]Apps\\[ApplicationFolderName]\t",
                "CustomAction:WixSetPerMachineFolder\t51\tAPPLICATIONFOLDER\t[WixPerMachineFolder]\t",
                "CustomAction:WixSetPerUserFolder\t51\tAPPLICATIONFOLDER\t[WixPerUserFolder]\t",
                "CustomAction:WixUIPrintEula_A64\t65\tWixUiCa_A64\tPrintEula\t",
                "CustomAction:WixUIValidatePath_A64\t65\tWixUiCa_A64\tValidatePath\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:AdvancedWelcomeEulaDlg\tPrint\tDoAction\tWixUIPrintEula_A64\t1\t1",
                "ControlEvent:BrowseDlg\tOK\tDoAction\tWixUIValidatePath_A64\tNOT WIXUI_DONTVALIDATEPATH\t1",
                "ControlEvent:InstallDirDlg\tNext\tDoAction\tWixUIValidatePath_A64\tNOT WIXUI_DONTVALIDATEPATH\t2",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).ToArray());
        }

        [Fact]
        public void CanBuildUsingWixUIFeatureTree()
        {
            var folder = TestData.Get(@"TestData", "WixUI_FeatureTree");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(BuildX64, "Binary", "Dialog", "CustomAction", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:WelcomeDlg\t"));
            Assert.Single(results, result => result.StartsWith("Dialog:CustomizeDlg\t"));
            Assert.Empty(results.Where(result => result.StartsWith("Dialog:SetupTypeDlg\t")));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_X64\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:SetWIXUI_EXITDIALOGOPTIONALTEXT\t51\tWIXUI_EXITDIALOGOPTIONALTEXT\tThank you for installing [ProductName].\t",
                "CustomAction:WixUIPrintEula_X64\t65\tWixUiCa_X64\tPrintEula\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:LicenseAgreementDlg\tPrint\tDoAction\tWixUIPrintEula_X64\t1\t1",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).ToArray());
        }

        [Fact]
        public void CanBuildUsingWixUIInstallDirWithCustomizedEula()
        {
            var folder = TestData.Get(@"TestData", "WixUI_InstallDir");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(BuildEula, "Binary", "Dialog", "CustomAction", "Property", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:InstallDirDlg\t"));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_X86\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:WixUIPrintEula_X86\t65\tWixUiCa_X86\tPrintEula\t",
                "CustomAction:WixUIValidatePath_X86\t65\tWixUiCa_X86\tValidatePath\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "Property:WIXUI_INSTALLDIR\tINSTALLFOLDER",
            }, results.Where(r => r.StartsWith("Property:WIXUI")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:BrowseDlg\tOK\tDoAction\tWixUIValidatePath_X86\tNOT WIXUI_DONTVALIDATEPATH\t3",
                "ControlEvent:InstallDirDlg\tNext\tDoAction\tWixUIValidatePath_X86\tNOT WIXUI_DONTVALIDATEPATH\t2",
                "ControlEvent:LicenseAgreementDlg\tPrint\tDoAction\tWixUIPrintEula_X86\t1\t1",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).OrderBy(s => s).ToArray());
        }

        [Fact]
        public void CanBuildUsingWixUIMinimal()
        {
            var folder = TestData.Get(@"TestData", "WixUI_Minimal");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(Build, "Binary", "Dialog", "CustomAction", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:WelcomeEulaDlg\t"));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_X86\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:WixUIPrintEula_X86\t65\tWixUiCa_X86\tPrintEula\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:WelcomeEulaDlg\tPrint\tDoAction\tWixUIPrintEula_X86\t1\t1",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).OrderBy(s => s).ToArray());
        }

        [Fact]
        public void CanBuildUsingWixUIMinimalInKazakh()
        {
            var folder = TestData.Get(@"TestData", "WixUI_Minimal");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(BuildInKazakh, "Dialog");
            var welcomeDlg = results.Where(r => r.StartsWith("Dialog:WelcomeDlg\t")).Select(r => r.Split('\t')).Single();
            Assert.Equal("[ProductName] бағдарламасын орнату", welcomeDlg[6]);
        }

        [Fact]
        public void CanBuildUsingWixUIMinimalAndReadPdb()
        {
            var folder = TestData.Get(@"TestData", "WixUI_Minimal");
            var bindFolder = TestData.Get(@"TestData", "data");

            using (var fs = new DisposableFileSystem())
            {
                var intermediateFolder = fs.GetFolder();

                Build(new[]
                {
                    "build",
                    Path.Combine(folder, "Package.wxs"),
                    "-ext", Path.GetFullPath(typeof(UIExtensionFactory).Assembly.Location),
                    "-bindpath", bindFolder,
                    "-intermediateFolder", intermediateFolder,
                    "-o", Path.Combine(intermediateFolder, @"bin\test.msi")
                });

                var wid = WindowsInstallerData.Load(Path.Combine(intermediateFolder, @"bin\test.wixpdb"));
                var dialogTable = wid.Tables["Dialog"];
                var dialogRow = dialogTable.Rows.Single(r => r.GetPrimaryKey() == "WelcomeEulaDlg");
            }
        }

        [Fact]
        public void CanBuildUsingWixUIMondo()
        {
            var folder = TestData.Get(@"TestData", "WixUI_Mondo");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(Build, "Binary", "Dialog", "CustomAction", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:WelcomeDlg\t"));
            Assert.Single(results, result => result.StartsWith("Dialog:CustomizeDlg\t"));
            Assert.Single(results, result => result.StartsWith("Dialog:SetupTypeDlg\t"));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_X86\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:WixUIPrintEula_X86\t65\tWixUiCa_X86\tPrintEula\t",
                "CustomAction:WixUIValidatePath_X86\t65\tWixUiCa_X86\tValidatePath\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:BrowseDlg\tOK\tDoAction\tWixUIValidatePath_X86\tNOT WIXUI_DONTVALIDATEPATH\t3",
                "ControlEvent:InstallDirDlg\tNext\tDoAction\tWixUIValidatePath_X86\tNOT WIXUI_DONTVALIDATEPATH\t2",
                "ControlEvent:LicenseAgreementDlg\tPrint\tDoAction\tWixUIPrintEula_X86\t1\t1",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).ToArray());
        }

        [Fact]
        public void CanBuildUsingWixUIMondoLocalized()
        {
            var folder = TestData.Get(@"TestData", "WixUI_Mondo");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(BuildInGerman, "Control");
            WixAssert.CompareLineByLine(new[]
            {
                "&Ja",
            }, results.Where(s => s.StartsWith("Control:ErrorDlg\tY")).Select(s => s.Split('\t')[9]).ToArray());
        }

        [Fact]
        public void CanBuildUsingWithInstallDirAndRemovedDialog()
        {
            var folder = TestData.Get(@"TestData", "InstallDir_NoLicense");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(Build, "Binary", "Dialog", "CustomAction", "Property", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:InstallDirDlg\t"));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_X86\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:WixUIValidatePath_X86\t65\tWixUiCa_X86\tValidatePath\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "Property:WIXUI_INSTALLDIR\tINSTALLFOLDER",
            }, results.Where(r => r.StartsWith("Property:WIXUI")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:BrowseDlg\tOK\tDoAction\tWixUIValidatePath_X86\tNOT WIXUI_DONTVALIDATEPATH\t3",
                "ControlEvent:InstallDirDlg\tNext\tDoAction\tWixUIValidatePath_X86\tNOT WIXUI_DONTVALIDATEPATH\t2",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).OrderBy(s => s).ToArray());

            Assert.Empty(results.Where(result => result.Contains("LicenseAgreementDlg")).ToArray());
        }

        [Fact]
        public void CanBuildUsingWithInstallDirAndAddedDialog()
        {
            var folder = TestData.Get(@"TestData", "InstallDir_SpecialDlg");
            var bindFolder = TestData.Get(@"TestData", "data");
            var build = new Builder(folder, typeof(UIExtensionFactory), new[] { bindFolder });

            var results = build.BuildAndQuery(BuildX64, "Binary", "Control", "Dialog", "CustomAction", "Property", "ControlEvent");
            Assert.Single(results, result => result.StartsWith("Dialog:InstallDirDlg\t"));
            WixAssert.CompareLineByLine(new[]
            {
                "Binary:WixUI_Bmp_Banner\t[Binary data]",
                "Binary:WixUI_Bmp_Dialog\t[Binary data]",
                "Binary:WixUI_Bmp_New\t[Binary data]",
                "Binary:WixUI_Bmp_Up\t[Binary data]",
                "Binary:WixUI_Ico_Exclam\t[Binary data]",
                "Binary:WixUI_Ico_Info\t[Binary data]",
                "Binary:WixUiCa_X64\t[Binary data]",
            }, results.Where(r => r.StartsWith("Binary:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "CustomAction:WixUIPrintEula_X64\t65\tWixUiCa_X64\tPrintEula\t",
                "CustomAction:WixUIValidatePath_X64\t65\tWixUiCa_X64\tValidatePath\t",
            }, results.Where(r => r.StartsWith("CustomAction:")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "Property:WIXUI_INSTALLDIR\tINSTALLFOLDER",
            }, results.Where(r => r.StartsWith("Property:WIXUI")).ToArray());
            WixAssert.CompareLineByLine(new[]
            {
                "ControlEvent:BrowseDlg\tOK\tDoAction\tWixUIValidatePath_X64\tNOT WIXUI_DONTVALIDATEPATH\t3",
                "ControlEvent:InstallDirDlg\tNext\tDoAction\tWixUIValidatePath_X64\tNOT WIXUI_DONTVALIDATEPATH\t2",
                "ControlEvent:LicenseAgreementDlg\tPrint\tDoAction\tWixUIPrintEula_X64\t1\t1",
            }, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("DoAction")).OrderBy(s => s).ToArray());

            Assert.Equal(8, results.Where(result => result.StartsWith("Control:") && result.Contains("SpecialDlg")).Count());
            Assert.Equal(5, results.Where(result => result.StartsWith("ControlEvent:") && result.Contains("SpecialDlg")).Count());
            Assert.Single(results.Where(result => result.StartsWith("Dialog:") && result.Contains("SpecialDlg")));
        }

        [Fact]
        public void CannotBuildWithV3LikeUIRef()
        {
            var folder = TestData.Get(@"TestData", "InvalidUIRef");

            using (var fs = new DisposableFileSystem())
            {
                var intermediateFolder = fs.GetFolder();
                var outputPath = Path.Combine(intermediateFolder, "bin", "test.msi");

                var args = new[]
                {
                    "build",
                    Path.Combine(folder, "Package.wxs"),
                    "-ext", typeof(UIExtensionFactory).Assembly.Location,
                    "-intermediateFolder", intermediateFolder,
                    "-o", outputPath,
                };

                var results = WixRunner.Execute(args);
                var message = results.Messages.Single();
                Assert.Equal("The identifier 'WixUI:WixUI_Mondo' is inaccessible due to its protection level.", message.ToString());
            }
        }

        private static void Build(string[] args)
        {
            var result = WixRunner.Execute(args)
                                  .AssertSuccess();
        }

        private static void BuildX64(string[] args)
        {
            var result = WixRunner.Execute(args.Concat(new[] { "-arch", "x64" }).ToArray())
                                  .AssertSuccess();
        }

        private static void BuildARM64(string[] args)
        {
            var result = WixRunner.Execute(args.Concat(new[] { "-arch", "arm64" }).ToArray())
                                  .AssertSuccess();
        }

        private static void BuildEula(string[] args)
        {
            var result = WixRunner.Execute(args.Concat(new[] { "-bv", "WixUILicenseRtf=bpl.rtf" }).ToArray())
                                  .AssertSuccess();
        }

        private static void BuildInGerman(string[] args)
        {
            var localizedArgs = args.Append("-culture").Append("de-DE").ToArray();

            var result = WixRunner.Execute(localizedArgs)
                                  .AssertSuccess();
        }

        private static void BuildInKazakh(string[] args)
        {
            var localizedArgs = args.Append("-culture").Append("kk-KZ").ToArray();

            var result = WixRunner.Execute(localizedArgs)
                                  .AssertSuccess();
        }
    }
}
