﻿using System;

namespace Wells.Tools
    #region Editor
    #endregion
    #region FolderBrowserDialog Base
    /// <summary>
        /// <summary>
        
        #region Public Property

        /// <summary>
        /// 窗体显示标题
        /// </summary>
        public string Description { get; set; }

        /// <summary>

                if (hr != 0)
        #endregion
        #region BaseType
            void SetFileTypes();  // not fully defined
            void SetFileTypeIndex([In] uint iFileType);
            void Unadvise();
            void ClearClientData();
            void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppsai); // not fully defined
        }
            void GetParent(); // not fully defined
            void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
            void Compare();  // not fully defined
        }
        #endregion
    #endregion