using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NZBlog.Common.Provider
{
    /**
     * 截取带html标签的字符串,再把标签补全(保证页面显示效果)
     * 一般是用在字符串中有html标签的截取.如: 后台发布用了在线编辑器, 前台显示内容要截取的情况.
     */
    public class SubStringHTML
    {
        /**
         * 按子节长度截取字符串(支持截取带HTML代码样式的字符串)<br>
         * 如：<span>中国人发在线</span> 当截取2个字节得到的结果是：<span>中国
         * 
         * @param param
         *            将要截取的含html代码的字符串参数
         * @param length
         *            截取的字节长度
         * @return 返回截取后的字符串
         */
        public static String subStringHTML(String param, int length)
        {
            StringBuilder result = new StringBuilder();
            int n = 0;
            char temp;
            bool isCode = false; // 是不是HTML代码
            bool isHTML = false; // 是不是HTML特殊字符,如&nbsp;
            for (int i = 0; i < param.Length; i++)
            {
                temp = param.ElementAt(i);
                if (temp == '<')
                {
                    isCode = true;
                }
                else if (temp == '&')
                {
                    isHTML = true;
                }
                else if (temp == '>' && isCode)
                {
                    n = n - 1;
                    isCode = false;
                }
                else if (temp == ';' && isHTML)
                {
                    isHTML = false;
                }
                if (!isCode && !isHTML)
                {
                    n = n + 1;
                    // UNICODE码字符占两个字节
                    if (Encoding.Default.GetBytes(temp + "").Length > 1)
                    {
                        n = n + 1;
                    }
                }
                result.Append(temp);
                if (n >= length)
                {
                    break;
                }
            }
            return fix(result.ToString());
        }
        /**
         * 补全HTML代码<br>
         * 如：<span>中国 ---> <span>中国</span>
         * 
         * @param str
         * @return
         * @author YangJunping
         * @date 2010-7-15
         */
        private static String fix(String str)
        {
            StringBuilder fixed1 = new StringBuilder(); // 存放修复后的字符串
            TagsList[] unclosedTags = getUnclosedTags(str);
            // 生成新字符串
            for (int i = unclosedTags[0].Size() - 1; i > -1; i--)
            {
                fixed1.Append("<" + unclosedTags[0].get(i) + ">");
            }
            fixed1.Append(str);
            for (int i = unclosedTags[1].Size() - 1; i > -1; i--)
            {
                String s = null;
                if ((s = unclosedTags[1].get(i)) != null)
                {
                    fixed1.Append("</" + s + ">");
                }
            }
            return fixed1.ToString();
        }
        private static TagsList[] getUnclosedTags(String str)
        {
            StringBuilder temp = new StringBuilder(); // 存放标签
            TagsList[] unclosedTags = new TagsList[2];
            unclosedTags[0] = new TagsList(); // 前不闭合，如有</div>而前面没有<div>
            unclosedTags[1] = new TagsList(); // 后不闭合，如有<div>而后面没有</div>
            bool flag = false; // 记录双引号"或单引号'
            char currentJump = ' '; // 记录需要跳过''还是""
            char current = ' ', last = ' '; // 当前 & 上一个
            // 开始判断
            for (int i = 0; i < str.Length; )
            {
                current = str.ElementAt(i++); // 读取一个字符
                if (current == '"' || current == '\'')
                {
                    flag = flag ? false : true; // 若为引号，flag翻转
                    currentJump = current;
                }
                if (!flag)
                {
                    if (current == '<')
                    { // 开始提取标签
                        current = str.ElementAt(i++);
                        if (current == '/')
                        { // 标签的闭合部分，如</div>
                            current = str.ElementAt(i++);
                            // 读取标签
                            while (i < str.Length && current != '>')
                            {
                                temp.Append(current);
                                current = str.ElementAt(i++);
                            }
                            // 从tags_bottom移除一个闭合的标签
                            if (!unclosedTags[1].remove(temp.ToString()))
                            { // 若移除失败，说明前面没有需要闭合的标签
                                unclosedTags[0].add(temp.ToString()); // 此标签需要前闭合
                            }
                            temp.Remove(0, temp.Length); // 清空temp
                        }
                        else
                        { // 标签的前部分，如<div>
                            last = current;
                            while (i < str.Length && current != ' ' && current != ' ' && current != '>')
                            {
                                temp.Append(current);
                                last = current;
                                current = str.ElementAt(i++);
                            }
                            // 已经读取到标签，跳过其他内容，如<div id=test>跳过id=test
                            while (i < str.Length && current != '>')
                            {
                                last = current;
                                current = str.ElementAt(i++);
                                if (current == '"' || current == '\'')
                                { // 判断引号
                                    flag = flag ? false : true;
                                    currentJump = current;
                                    if (flag)
                                    { // 若引号不闭合，跳过到下一个引号之间的内容
                                        while (i < str.Length && str.ElementAt(i++) != currentJump)
                                            ;
                                        current = str.ElementAt(i++);
                                        flag = false;
                                    }
                                }
                            }
                            if (last != '/' && current == '>') // 判断这种类型：<TagName />
                                unclosedTags[1].add(temp.ToString());
                            temp.Remove(0, temp.Length);
                        }
                    }
                }
                else
                {
                    while (i < str.Length && str.ElementAt(i++) != currentJump)
                        ; // 跳过引号之间的部分
                    flag = false;
                }
            }
            return unclosedTags;
        }
    }
    public class TagsList
    {
        private String[] data;
        private int size = 0;
        public TagsList(int size)
        {
            data = new String[size];
        }
        public TagsList()
            : this(10)
        {

        }
        public void add(String str)
        {
            ensureCapacity(size + 1);
            data[size++] = str;
        }
        public String get(int index)
        {
            if (index < size)
                return data[index];
            else
                return null;
        }
        // 为了提高效率，只将其置为null
        public bool remove(String str)
        {
            for (int index = 0; index < size; index++)
            {
                if (str.Equals(data[index]))
                {
                    data[index] = null;
                    return true;
                }
            }
            return false;
        }
        public bool remove(int index)
        {
            if (index < data.Length)
            {
                data[index] = null;
                return true;
            }
            return false;
        }
        public int Size()
        {
            return this.size;
        }
        // 扩展容量
        public void ensureCapacity(int minSize)
        {
            int oldCapacity = data.Length;
            if (minSize > oldCapacity)
            {
                int newCapacity = (oldCapacity * 3 / 2 + 1) > minSize ? oldCapacity * 3 / 2 + 1 : minSize;
                String[] newArray = new String[newCapacity];
                for (int i = 0; i < data.Length; i++)
                {
                    newArray[i] = data[i];
                }
                data = newArray;
            }
        }
    }
}
