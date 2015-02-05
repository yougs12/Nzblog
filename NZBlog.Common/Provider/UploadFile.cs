﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

namespace NZBlog.Common
{
    public class UploadFile
    {
        public string UploadImg(HttpResponseBase Response, HttpRequestBase Request)
        {
            Response.Charset = "UTF-8";

            // 初始化一大堆变量
            string inputname = "filedata";//表单文件域name
            string attachdir = "/upload";     // 上传文件保存路径，结尾不要带/
            int dirtype = 1;                 // 1:按天存入目录 2:按月存入目录 3:按扩展名存目录  建议使用按天存
            int maxattachsize = 2097152;     // 最大上传大小，默认是2M
            string upext = "jpg,jpeg,gif,png";    // 上传扩展名
            int msgtype = 2;                 //返回上传参数的格式：1，只返回url，2，返回参数数组
            string immediate = Request.QueryString["immediate"];//立即上传模式，仅为演示用
            byte[] file;                     // 统一转换为byte数组处理
            string localname = "";
            string disposition = Request.ServerVariables["HTTP_CONTENT_DISPOSITION"];

            string err = "";
            string msg = "''";

            if (disposition != null)
            {
                // HTML5上传
                file = Request.BinaryRead(Request.TotalBytes);
                localname = HttpContext.Current.Server.UrlDecode(Regex.Match(disposition, "filename=\"(.+?)\"").Groups[1].Value);// 读取原始文件名
            }
            else
            {
                HttpFileCollectionBase filecollection = Request.Files;
                HttpPostedFileBase postedfile = filecollection.Get(inputname);

                // 读取原始文件名
                localname = postedfile.FileName;
                // 初始化byte长度.
                file = new Byte[postedfile.ContentLength];

                // 转换为byte类型
                System.IO.Stream stream = postedfile.InputStream;
                
                stream.Read(file, 0, postedfile.ContentLength);
                stream.Close();

                filecollection = null;
            }

            if (file.Length == 0) err = "无数据提交";
            else
            {
                if (file.Length > maxattachsize) err = "文件大小超过" + maxattachsize + "字节";
                else
                {
                    string attach_dir, attach_subdir, filename, extension, target;

                    // 取上载文件后缀名
                    extension = GetFileExt(localname);

                    if (("," + upext + ",").IndexOf("," + extension + ",") < 0) err = "上传文件扩展名必需为：" + upext;
                    else
                    {
                        System.IO.Stream stream1 = new MemoryStream(file);
                        //Image img = Image.FromStream(stream1);
                        switch (dirtype)
                        {
                            case 2:
                                attach_subdir = "month_" + DateTime.Now.ToString("yyMM");
                                break;
                            case 3:
                                attach_subdir = "ext_" + extension;
                                break;
                            default:
                                attach_subdir = "day_" + DateTime.Now.ToString("yyMMdd");
                                break;
                        }
                        attach_dir = attachdir + "/" + attach_subdir + "/";

                        // 生成随机文件名
                        Random random = new Random(DateTime.Now.Millisecond);
                        filename = DateTime.Now.ToString("yyyyMMddhhmmss") + random.Next(10000) + "." + extension;

                        target = attach_dir + filename;
                        try
                        {
                            CreateFolder(HttpContext.Current.Server.MapPath(attach_dir));

                            NZBlog.Common.ImageHelper.ZoomAuto(stream1, HttpContext.Current.Server.MapPath(target), 600, 600, "zhengxueping.com", "");
                        }
                        catch (Exception ex)
                        {
                            err = ex.Message.ToString();
                        }

                        // 立即模式判断
                        if (immediate == "1") target = "!" + target;
                        target = jsonString(target);
                        if (msgtype == 1) msg = "'" + target + "'";
                        else msg = "{'url':'" + target + "','localname':'" + jsonString(localname) + "','id':'1'}";
                    }
                }
            }

            file = null;

            return "{'err':'" + jsonString(err) + "','msg':" + msg + "}";
        }


        string jsonString(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("/", "\\/");
            str = str.Replace("'", "\\'");
            return str;
        }


        string GetFileExt(string FullPath)
        {
            if (FullPath != "") return FullPath.Substring(FullPath.LastIndexOf('.') + 1).ToLower();
            else return "";
        }

        void CreateFolder(string FolderPath)
        {
            if (!System.IO.Directory.Exists(FolderPath)) System.IO.Directory.CreateDirectory(FolderPath);
        }
    }
}