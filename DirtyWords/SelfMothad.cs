using KudyStudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirtyWords
{
    class SelfMothad
    {
        //static void Main(string[] args)
        //{

        //}

        public string GetHandleDirtyWords(string wordInput)
        {
            // 设置了脏字中间夹杂的的特殊字符
            string skipChars = " 　=_+═十▇▆■ ▓ 回 □ 〓≡ ╝╚╔ ╗╬ ═ ╓ ╩ ┠ ┨┯ ┷┏ ┓┗ ┛┳⊥『』┌ ┐└ ┘∟「」↑↓→←↘↙♀♂┇┅ ﹉﹊﹍﹎╭ ╮╰ ╯ *^_^* ^*^ ^-^ ^_^ ^（^ ∵∴∥｜ ｜︴﹏﹋﹌（）〔〕 【】〖〗＠：！/_< >`,．。≈{}~ ～()_-『』√ $ @ * & # ※ 卐 々∞Ψ ∪∩∈∏の℡ぁ§∮”〃ミ灬ξ№∑⌒ξζω＊??ㄨ ≮≯ ＋ －×÷＋－±／＝∫∮∝ ∞ ∧∨ ∑ ∏ ∥∠ ≌ ∽ ≤ ≥ ≈＜＞じ ξζω□∮〓※∴ぷ▂▃▅▆█ ∏卐【】△√ ∩¤々♀♂∞①ㄨ≡↘↙▂ ▂ ▃ ▄ ▅ ▆ ▇ █┗┛╰☆╮ ≠ ▂ ▃ ▄ ▅ ";
            List<string> keywords = new List<string>();
            // 从文件读取脏字
            foreach (string line in File.ReadAllLines(EnvironmentUtil.GetFullPath("DirtyWordsDictionary.txt")))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    keywords.Add(line);
                }
            }
            KeywordFilterResult result;
            result = KeywordFilter.Filter(wordInput, keywords, skipChars, KeywordFormatter.ToIterantStar, KeywordOrder.Descending, true/* ignoreCase */);
            //KeywordFilter filter = new KeywordFilter(keywords, skipChars);
            //result = filter.Filter(wordInput, KeywordFormatter.ToIterantStar, KeywordOrder.Descending, true/* ignoreCase */);
            return result.Result;
        }
    }
}
