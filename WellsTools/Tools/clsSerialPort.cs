using System;
using System.Runtime.InteropServices;


namespace Wells.Tools
{
    /// <summary>
    /// ����ͨѶ��
    /// </summary>
    public class clsSerialPort
    {
        #region WINAPI����
        /// <summary>
        /// д��־
        /// </summary>
        private const uint GENERIC_READ = 0x80000000;
        
        /// <summary>
        /// ����־
        /// </summary>
        private const uint GENERIC_WRITE = 0x40000000;
        
        /// <summary>
        /// ���Ѵ���
        /// </summary>
        private const int OPEN_EXISTING = 3;
        
        /// <summary>
        /// ��Ч���
        /// </summary>
        private const int INVALID_HANDLE_VALUE = -1;
        #endregion

        #region ��Ա����
        /// <summary>
        /// �˿�����(COM1,COM2...COM4...)
        /// </summary>
        public int PortNum;
        
        /// <summary>
        /// ������9600
        /// </summary>
        public int BaudRate;
        
        /// <summary>
        /// ����λ4-8
        /// </summary>
        public byte ByteSize;
        
        /// <summary>
        /// ��żУ��0-4=no,odd,even,mark,space
        /// </summary>
        public byte Parity;
        
        /// <summary>
        /// ֹͣλ
        /// </summary>
        public byte StopBits; // 0,1,2 = 1, 1.5, 2

        /// <summary>
        /// ��ʱ��
        /// </summary>
        public int ReadTimeout;
        
        /// <summary>
        /// COM�ھ��
        /// </summary>
        private int hComm = INVALID_HANDLE_VALUE;
        
        /// <summary>
        /// �����Ƿ��Ѿ���
        /// </summary>
        public bool Opened = false;
        #endregion

        #region �豸���ƿ�ṹ������
        /// <summary>
        /// �豸���ƿ�ṹ������
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DCB
        {
            /// <summary>
            /// DCB����
            /// </summary>
            public int DCBlength;
            
            /// <summary>
            /// ָ����ǰ������
            /// </summary>
            public int BaudRate;
            
            /// <summary>
            /// ָ���Ƿ����������ģʽ
            /// </summary>
            public int fBinary;
            
            /// <summary>
            /// ָ���Ƿ�������żУ��
            /// </summary>
            public int fParity;
            
            /// <summary>
            /// ָ��CTS�Ƿ����ڼ�ⷢ�Ϳ��ƣ���ΪTRUE��CTSΪOFF�����ͽ�������
            /// </summary>
            public int fOutxCtsFlow;
            
            /// <summary>
            /// ָ��CTS�Ƿ����ڼ�ⷢ�Ϳ���
            /// </summary>
            public int fOutxDsrFlow;
            
            /// <summary>
            /// DTR_CONTROL_DISABLEֵ��DTR��ΪOFF, DTR_CONTROL_ENABLEֵ��DTR��ΪON, DTR_CONTROL_HANDSHAKE����DTR"����"
            /// </summary>
            public int fDtrControl;
            
            /// <summary>
            /// ����ֵΪTRUEʱDSRΪOFFʱ���յ��ֽڱ�����
            /// </summary>
            public int fDsrSensitivity;
            
            /// <summary>
            /// ָ�������ջ���������,�������������Ѿ����ͳ�XoffChar�ַ�ʱ�����Ƿ�ֹͣ��
            /// TRUEʱ���ڽ��ջ��������յ��������������ֽ�XoffLim�����������Ѿ����ͳ�
            /// XoffChar�ַ���ֹ�����ֽ�֮�󣬷��ͼ������С���FALSEʱ���ڽ��ջ�������
            /// �յ����������ѿյ��ֽ�XonChar�����������Ѿ����ͳ��ָ����͵�XonChar֮
            /// �󣬷��ͼ������С�XOFF continues Tx
            /// </summary>
            public int fTXContinueOnXoff;
            
            /// <summary>
            /// TRUEʱ�����յ�XoffChar֮���ֹͣ���ͽ��յ�XonChar֮�����¿�ʼ XON/XOFF
            /// out flow control
            /// </summary>
            public int fOutX;
            
            /// <summary>
            /// TRUEʱ�����ջ��������յ�������������XoffLim֮��XoffChar���ͳ�ȥ����
            /// ���������յ����������յ�XonLim֮��XonChar���ͳ�ȥ XON/XOFF in flow control
            /// </summary>
            public int fInX;
            
            /// <summary>
            /// ��ֵΪTRUE��fParityΪTRUEʱ����ErrorChar ��Աָ�����ַ�������żУ�����
            /// �Ľ����ַ� enable error replacement
            /// </summary>
            public int fErrorChar;
            
            /// <summary>
            /// eTRUEʱ������ʱȥ���գ�0ֵ���ֽ� enable null stripping
            /// </summary>
            public int fNull;
            
            /// <summary>
            /// RTS flow control RTS_CONTROL_DISABLEʱ,RTS��ΪOFF RTS_CONTROL_ENABLEʱ, RTS��ΪON
            /// RTS_CONTROL_HANDSHAKEʱ,�����ջ�����С�ڰ���ʱRTSΪON�����ջ����������ķ�֮
            /// ����ʱRTSΪOFF RTS_CONTROL_TOGGLEʱ,�����ջ���������ʣ���ֽ�ʱRTSΪON ,����
            /// ȱʡΪOFF
            /// </summary>
            public int fRtsControl;
            
            /// <summary>
            /// TRUEʱ,�д�����ʱ��ֹ����д���� abort on error
            /// </summary>
            public int fAbortOnError;
            
            /// <summary>
            /// δʹ��
            /// </summary>
            public int fDummy2;
            
            /// <summary>
            /// ��־λ
            /// </summary>
            public uint flags;
            
            /// <summary>
            /// δʹ��,����Ϊ0
            /// </summary>
            public ushort wReserved;
            
            /// <summary>
            /// ָ����XON�ַ�������ǰ���ջ������п��������С�ֽ���
            /// </summary>
            public ushort XonLim;
            
            /// <summary>
            /// ָ����XOFF�ַ�������ǰ���ջ������п��������С�ֽ���
            /// </summary>
            public ushort XoffLim;
            
            /// <summary>
            /// ָ���˿ڵ�ǰʹ�õ�����λ
            /// </summary>
            public byte ByteSize;
            
            /// <summary>
            /// ָ���˿ڵ�ǰʹ�õ���żУ�鷽��,����Ϊ:EVENPARITY,MARKPARITY,NOPARITY,ODDPARITY 0-4=no,odd,even,mark,space
            /// </summary>
            public byte Parity;
            
            /// <summary>
            /// ָ���˿ڵ�ǰʹ�õ�ֹͣλ��,����Ϊ:ONESTOPBIT,ONE5STOPBITS,TWOSTOPBITS 0,1,2 = 1, 1.5, 2
            /// </summary>
            public byte StopBits;
            
            /// <summary>
            /// ָ�����ڷ��ͺͽ����ַ�XON��ֵ Tx and Rx XON character
            /// </summary>
            public char XonChar;
            
            /// <summary>
            /// ָ�����ڷ��ͺͽ����ַ�XOFFֵ Tx and Rx XOFF character
            /// </summary>
            public char XoffChar;
            
            /// <summary>
            /// ���ַ�����������յ�����żУ�鷢������ʱ��ֵ
            /// </summary>
            public char ErrorChar;
            
            /// <summary>
            /// ��û��ʹ�ö�����ģʽʱ,���ַ�������ָʾ���ݵĽ���
            /// </summary>
            public char EofChar;
            
            /// <summary>
            /// �����յ����ַ�ʱ,�����һ���¼�
            /// </summary>
            public char EvtChar;
            
            /// <summary>
            /// δʹ��
            /// </summary>
            public ushort wReserved1;
        }
        #endregion

        #region ���ڳ�ʱʱ��ṹ������
        /// <summary>
        /// ���ڳ�ʱʱ��ṹ������
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct COMMTIMEOUTS
        {
            public int ReadIntervalTimeout;
            public int ReadTotalTimeoutMultiplier;
            public int ReadTotalTimeoutConstant;
            public int WriteTotalTimeoutMultiplier;
            public int WriteTotalTimeoutConstant;
        }
        #endregion

        #region ����������ṹ������
        /// <summary>
        /// ����������ṹ������
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            public int Internal;
            public int InternalHigh;
            public int Offset;
            public int OffsetHigh;
            public int hEvent;
        }
        #endregion

        #region ����winAPI����
        /// <summary>
        /// �򿪴���
        /// </summary>
        /// <param name="lpFileName">Ҫ�򿪵Ĵ�������</param>
        /// <param name="dwDesiredAccess">ָ�����ڵķ��ʷ�ʽ��һ������Ϊ�ɶ���д��ʽ</param>
        /// <param name="dwShareMode">ָ�����ڵĹ���ģʽ�����ڲ��ܹ�����������Ϊ0</param>
        /// <param name="lpSecurityAttributes">���ô��ڵİ�ȫ���ԣ�WIN9X�²�֧�֣�Ӧ��ΪNULL</param>
        /// <param name="dwCreationDisposition">���ڴ���ͨ�ţ�������ʽֻ��ΪOPEN_EXISTING</param>
        /// <param name="dwFlagsAndAttributes">ָ�������������־������ΪFILE_FLAG_OVERLAPPED(�ص�I/O����)��ָ���������첽��ʽͨ��</param>
        /// <param name="hTemplateFile">���ڴ���ͨ�ű�������ΪNULL</param>
        [DllImport("kernel32.dll")]
        private static extern int CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode,int lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);
        
        /// <summary>
        /// �õ�����״̬
        /// </summary>
        /// <param name="hFile">ͨ���豸���</param>
        /// <param name="lpDCB">�豸���ƿ�DCB</param>
        [DllImport("kernel32.dll")]
        private static extern bool GetCommState(int hFile, ref DCB lpDCB);
        
        /// <summary>
        /// ���������豸���ƿ�
        /// </summary>
        /// <param name="lpDef">�豸�����ַ���</param>
        /// <param name="lpDCB">�豸���ƿ�</param>
        [DllImport("kernel32.dll")]
        private static extern bool BuildCommDCB(string lpDef, ref DCB lpDCB);
        
        /// <summary>
        /// ���ô���״̬
        /// </summary>
        /// <param name="hFile">ͨ���豸���</param>
        /// <param name="lpDCB">�豸���ƿ�</param>
        [DllImport("kernel32.dll")]
        private static extern bool SetCommState(int hFile, ref DCB lpDCB);
        
        /// <summary>
        /// ��ȡ���ڳ�ʱʱ��
        /// </summary>
        /// <param name="hFile">ͨ���豸���</param>
        /// <param name="lpCommTimeouts">��ʱʱ��</param>
        [DllImport("kernel32.dll")]
        private static extern bool GetCommTimeouts(int hFile, ref COMMTIMEOUTS lpCommTimeouts);
        
        /// <summary>
        /// ���ô��ڳ�ʱʱ��
        /// </summary>
        /// <param name="hFile">ͨ���豸���</param>
        /// <param name="lpCommTimeouts">��ʱʱ��</param>
        [DllImport("kernel32.dll")]
        private static extern bool SetCommTimeouts(int hFile, ref COMMTIMEOUTS lpCommTimeouts);
        
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="hFile">ͨ���豸���</param>
        /// <param name="lpBuffer">���ݻ�����</param>
        /// <param name="nNumberOfBytesToRead">�����ֽڵȴ���ȡ</param>
        /// <param name="lpNumberOfBytesRead">��ȡ�����ֽ�</param>
        /// <param name="lpOverlapped">���������</param>
        [DllImport("kernel32.dll")]
        private static extern bool ReadFile(int hFile, byte[] lpBuffer,
                                          int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, ref OVERLAPPED lpOverlapped);
        
        /// <summary>
        /// д��������
        /// </summary>
        /// <param name="hFile">ͨ���豸���</param>
        /// <param name="lpBuffer">���ݻ�����</param>
        /// <param name="nNumberOfBytesToWrite">�����ֽڵȴ�д��</param>
        /// <param name="lpNumberOfBytesWritten">�Ѿ�д������ֽ�</param>
        /// <param name="lpOverlapped">���������</param>
        [DllImport("kernel32.dll")]
        private static extern bool WriteFile(int hFile, byte[] lpBuffer,
                                          int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, ref OVERLAPPED lpOverlapped);

        /// <summary>
        /// ͬ����մ��ڻ�����
        /// </summary>
        /// <param name="hFile"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FlushFileBuffers(int hFile);

        /// <summary>
        /// ��մ��ڻ�����
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool PurgeComm(int hFile, uint dwFlags);
        
        /// <summary>
        /// �رմ���
        /// </summary>
        /// <param name="hObject">ͨ���豸���</param>
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int hObject);
        
        /// <summary>
        /// �õ��������һ�η��صĴ���
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();
        #endregion

        #region �����봮�ڵ�����
        /// <summary>
        /// �����봮�ڵ�����
        /// </summary>
        public bool open()
        {
            DCB dcbCommPort = new DCB();
            COMMTIMEOUTS ctoCommPort = new COMMTIMEOUTS();
            // �򿪴���
            hComm = CreateFile("COM" + PortNum, GENERIC_READ | GENERIC_WRITE,0, 0, OPEN_EXISTING, 0, 0);
            if (hComm == INVALID_HANDLE_VALUE)
            {
                return false;
            }
            // ����ͨ�ų�ʱʱ��
            GetCommTimeouts(hComm, ref ctoCommPort);
            ctoCommPort.ReadTotalTimeoutConstant = ReadTimeout;
            ctoCommPort.ReadTotalTimeoutMultiplier = 0;
            ctoCommPort.WriteTotalTimeoutMultiplier = 0;
            ctoCommPort.WriteTotalTimeoutConstant = 0;
            SetCommTimeouts(hComm, ref ctoCommPort);
            // ���ô���
            GetCommState(hComm, ref dcbCommPort);
            dcbCommPort.fOutxCtsFlow = 524800;
            dcbCommPort.BaudRate = BaudRate;
            dcbCommPort.flags = 0;
            dcbCommPort.flags |= 1;
            if (Parity > 0)
            {
                dcbCommPort.flags |= 2;
            }
            dcbCommPort.Parity = Parity;
            dcbCommPort.ByteSize = ByteSize;
            dcbCommPort.StopBits = StopBits;
            dcbCommPort.fOutxCtsFlow = 524800;
            if (!SetCommState(hComm, ref dcbCommPort))
            {
                return false;
            }
            Opened = true;
            return true;
        }
        #endregion
        
        #region �رմ���,����ͨѶ
        /// <summary>
        /// �رմ���,����ͨѶ
        /// </summary>
        public bool close()
        {
            if (hComm != INVALID_HANDLE_VALUE)
            {
                CloseHandle(hComm);
                Opened = false;
                return true;
            }
            return false;
        }
        #endregion
        
        #region ��ȡ���ڷ��ص�����
        /// <summary>
        /// ��ȡ���ڷ��ص�����
        /// </summary>
        /// <param name="NumBytes">���ݳ���</param>
        public byte[] read(int NumBytes)
        {
            byte[] BufBytes;
            byte[] OutBytes;
            BufBytes = new byte[NumBytes];
            if (hComm != INVALID_HANDLE_VALUE)
            {
                OVERLAPPED ovlCommPort = new OVERLAPPED();
                int BytesRead = 0;

                ReadFile(hComm, BufBytes, NumBytes, ref BytesRead, ref ovlCommPort);
                OutBytes = new byte[BytesRead];
                Array.Copy(BufBytes, OutBytes, BytesRead);
                return OutBytes;
            }
            else
            {
                return new byte[0];
            }
        }
        #endregion
        
        #region ���COM�ڻ���������
        /// <summary>
        /// ���COM�ڻ���������
        /// </summary>
        /// <returns></returns>
        public bool clearPortData()
        {
            if (hComm != INVALID_HANDLE_VALUE)
            {
                return PurgeComm(hComm, 0);
            }
            return false;
        }
        #endregion
        
        #region �򴮿�д����
        /// <summary>
        /// �򴮿�д����
        /// </summary>
        /// <param name="WriteBytes">��������</param>
        public int write(byte[] WriteBytes)
        {
            if (hComm != INVALID_HANDLE_VALUE)
            {
                OVERLAPPED ovlCommPort = new OVERLAPPED();
                int BytesWritten = 0;
                WriteFile(hComm, WriteBytes, WriteBytes.Length,
                 ref BytesWritten, ref ovlCommPort);
                return BytesWritten;
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}