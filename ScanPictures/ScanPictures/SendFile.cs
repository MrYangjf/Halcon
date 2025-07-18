using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSendFile
{
    public class SendFile
    {
        public delegate void ErrMesHandler(string mes);
        public event ErrMesHandler ErrMes;
        public bool IsConnect;
        public bool ConnectState(string path,string userName,string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                path ="\\\\" +  path.Split("\\"[0])[2];
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = " net use " + path + " " + passWord + " /user:" + userName;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"局域网通讯异常:\r\n{ex.Message}");
                if (ErrMes != null) ErrMes(ex.Message);
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            IsConnect = Flag;
            return Flag;
        }
        /// <summary>
        /// 复制文件夹下的所有文件（不包括子文件夹）
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="desDir"></param>
        public void CopyFileAndDir(string dir,string desDir)
        {
            if(!System.IO.Directory.Exists(desDir))
            {
                System.IO.Directory.CreateDirectory(desDir);

            }
            IEnumerable<string> files = System.IO.Directory.EnumerateFileSystemEntries(dir);
            if (files != null && files.Count() > 0)
            {
                foreach (var item in files)
                {
                    string desPath = System.IO.Path.Combine(desDir, System.IO.Path.GetFileName(item));
                    var fileExist = System.IO.File.Exists(item);
                    if (fileExist)
                    {
                        System.IO.File.Copy(item, desPath, true);
                        continue;
                    }
                }
            }

        }
        public bool CopyFile(string dir, string desDir)
        {
            try
            {
                string strDesDir = "";
                strDesDir = desDir.Replace("\\" + desDir.Split("\\"[0]).Last(), "");
                if (!System.IO.Directory.Exists(strDesDir))
                {
                    System.IO.Directory.CreateDirectory(strDesDir);
                }
                System.IO.File.Copy(dir, desDir, true);
                return true;
            }
            catch (Exception ex)
            {
                if(ErrMes != null)
                {
                    ErrMes(ex.Message);
                }
                return false;
            }
            
        }
    }

    
}
