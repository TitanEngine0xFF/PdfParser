using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace pdfExtrator.Common
{
    public static class DirectoryInfoExt
    {

        public static Tuple< List<FileInfo>,List<DirectoryInfo>> EnumerateDirTreeAllfilesDirs(this System.IO.DirectoryInfo di)
        {
            Stack<Tuple<DirectoryInfo[], int>> stack = new Stack<Tuple<DirectoryInfo[], int>>();



            var listtmp = di.GetFiles("*.pdf").ToList();
            List<DirectoryInfo> dixx = new List<DirectoryInfo>();
            dixx.AddRange(di.GetDirectories());

            stack.Push(new Tuple<DirectoryInfo[], int>(di.GetDirectories(), 0));
        reScan:
            var tmp = stack.Pop();

            var dirs = tmp.Item1;

            for (int i = tmp.Item2; i < dirs.Length; i++)
            {
                listtmp.AddRange(dirs[i].GetFiles("*.pdf").ToList());

                if (dirs[i].GetDirectories().Length > 0)
                {
                    dixx.AddRange(dirs[i].GetDirectories());
                    //这里要压栈2次,一次是展开, 一次是还原上层循环
                    stack.Push(new Tuple<DirectoryInfo[], int>(dirs[i].GetDirectories(), i));
                    stack.Push(new Tuple<DirectoryInfo[], int>(dirs, i+1 ));
                    goto reScan;
                }
            }

            if (stack.Count > 0)
            {
                goto reScan;
            }

            return new Tuple<List<FileInfo>, List<DirectoryInfo>>(listtmp, dixx);
        }

        
    }

    public class FileHelper
    {
        public Tuple<List<string>, List<DirectoryInfo>> listfileInfo = new Tuple<List<string>, List<DirectoryInfo>>(new List<string> (10), new List<DirectoryInfo> (10));

        bool isFrist = true;
        public Tuple<List<string>, List<DirectoryInfo>> GetDirAllFiles(string dir1)
        {
            List<string> list = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(dir1);

            if (isFrist)
            {
                foreach (var item in dir.GetFiles("*.pdf"))
                {
                    listfileInfo.Item1.Add(item.FullName);
                }
                isFrist = false;
            }

            DirectoryInfo[] dirinfo = dir.GetDirectories();

            for (int i = 0; i < dirinfo.Length; i++)
            {
                listfileInfo.Item1.AddRange(Directory.GetFiles(dirinfo[i].FullName, "*.pdf"));
                listfileInfo.Item2.Add(dirinfo[i]);
                GetDirAllFiles(dirinfo[i].FullName);
            }

            //for (int i = 0; i < list.Count; i++)
            //{
            //    Console.WriteLine(list[i]);
            //}

            return listfileInfo;

        }

    }
}
