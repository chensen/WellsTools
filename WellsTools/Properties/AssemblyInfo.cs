using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle(WellsAssembly.Title)]
[assembly: AssemblyDescription(WellsAssembly.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(WellsAssembly.Company)]
[assembly: AssemblyProduct(WellsAssembly.Product)]
[assembly: AssemblyCopyright(WellsAssembly.Copyright)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//将 ComVisible 设置为 false 将使此程序集中的类型
//对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("f0245c7e-7fa8-4a3e-9237-59567ccd5562")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
//可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值，
// 方法是按如下所示使用“*”: :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(WellsAssembly.Version)]
[assembly: AssemblyFileVersion(WellsAssembly.Version)]

internal static class WellsAssembly
{
    internal const string Title = "Wells.dll";
    internal const string Version = "1.0.0.0";
    internal const string Description = "Wells Personal Tools";
    internal const string Copyright = "Copyright  2017 Wells.  All rights reserved.";
    internal const string Company = "Siyar";
    internal const string Product = "Wells";
}

namespace Wells.WellsFramework
{
    internal static class AssemblyRef
    {

        // Design

        internal const string MetroFrameworkDesign_ = "Wells.WellsFramework.Design";

        internal const string MetroFrameworkDesignSN = "Wells.WellsFramework.Design, Version=" + WellsAssembly.Version
                                                       + ", Culture=neutral, PublicKeyToken=null";// + MetroFrameworkKeyToken;

        internal const string MetroFrameworkDesignIVT = "Wells.WellsFramework.Design";//, PublicKey=" + MetroFrameworkKeyFull;

        // Fonts

        internal const string MetroFrameworkFonts_ = "Wells.WellsFramework.Fonts";

        internal const string MetroFrameworkFontsSN = "Wells.WellsFramework.Fonts, Version=" + WellsAssembly.Version
                                                      + ", Culture=neutral, PublicKeyToken=null";// + MetroFrameworkKeyToken;

        internal const string MetroFrameworkFontsIVT = "Wells.WellsFramework.Fonts";//, PublicKey=" + MetroFrameworkKeyFull;

        internal const string MetroFrameworkFontResolver = "Wells.WellsFramework.Fonts.FontResolver, " + MetroFrameworkFontsSN;

        // Strong Name Key

        internal const string MetroFrameworkKey = "5f91a84759bf584a";

        internal const string MetroFrameworkKeyFull =
            "00240000048000009400000006020000002400005253413100040000010001004d3b6f2adab21d" +
            "00d59de966f5d7f4d8325296ded578ac35bca529580b534443bb4090600ff1f057136d58f20a22" +
            "5e0d025119aec656e9b6ac5691e12689c0b03d55c8b8822fd84e2acbde80a2d9124009d20f5adf" +
            "05d36cfa95ba164a0d6ab348a9f8e3a00f066f4d32c0b71b5be6d7f86616491f6dd0630e49ec15" +
            "a0c8f9c9";

        internal const string MetroFrameworkKeyToken = "5f91a84759bf584a";
    }
}
