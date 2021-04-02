
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set 
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly:ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                             //(used if a resource is not found in the page, 
                             // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                      //(used if a resource is not found in the page, 
                                      // app, or any theme specific resource dictionaries)
)]

[assembly: AssemblyKeyFile(@"..\LVCSign.snk")]
[assembly: InternalsVisibleTo("LiveCharts.Geared,PublicKey=0024000004800000940000000602000000240000525341310004000001000100bd2e66fab8ce9a4900047ffda57d2f525cf6313dcc9d20994c5e6b3cd8fca906ba7ccd54bea5f7bd6cb503deb81d685259e355e3a9b5c21e5bc80091f08846246371b2a71ab306655651261e910adfa61be236b77d11df23a44d48f00a0e07c689f9a2daaff16d505a1c861d9854d92ed5a8a38fb28c1343fb691462873e71a1")]
[assembly: InternalsVisibleTo("LiveCharts.WinForms,PublicKey=002400000480000094000000060200000024000052534131000400000100010009132ae1a474e2ecf9903c1ef8945a2119aa0b9a3b4e40c43f6cb66233669e3007b4109d5c37957c2d0c5cfe7fce34366150210f83c618c18cdc8d7b763bff60419837a2185df1867c73f679e05f82b861e6ca764612eabc36d71858260b262bb9c3ad546c9692fe6379a520b6c5fc701e0ee6d071b52e9f20166fc0752ff894")]
