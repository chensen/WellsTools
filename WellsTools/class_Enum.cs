using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wells
{
    public enum FormCloseType
    {
        None = 0,
        Close = 1,
        Restart = 2,
        CloseSystem = 3,
        RestartSystem = 4,
    }

    public enum iDrawMode
    {
        Default = 0,
        Normal = 1,
        Error = 2,
    }

    public enum iShowMode
    {
        None = 0,
        //
        // 摘要:
        //     该消息框包含一个符号，该符号是由一个红色背景的圆圈及其中的白色 X 组成的。
        Error = 16,
        //
        // 摘要:
        //     该消息框包含一个符号，该符号是由一个圆圈和其中的一个问号组成的。不再建议使用问号消息图标，原因是该图标无法清楚地表示特定类型的消息，并且问号形式的消息表述可应用于任何消息类型。此外，用户还可能将问号消息符号与帮助信息混淆。因此，请不要在消息框中使用此问号消息符号。系统继续支持此符号只是为了向后兼容。
        Question = 32,
        //
        // 摘要:
        //     该消息框包含一个符号，该符号是由一个黄色背景的三角形及其中的一个感叹号组成的。
        Warning = 48,
        //
        // 摘要:
        //     该消息框包含一个符号，该符号是由一个圆圈及其中的小写字母 i 组成的。
        Asterisk = 64,
        //
        // 摘要:
        //     该消息框包含一个符号，该符号是由一个圆圈及其中的小写字母 i 组成的。
        Information = 80,
    }
}
