﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using KudyStudio;

namespace KudyStudioFilteringTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestKeywordFilter.FilterTest1();
            TestKeywordFilter.FilterTest2();
            TestKeywordFilter.FilterTest3();
            TestKeywordFilter.HighlightTest();
        }
    }

    /// <summary>
    /// 自定义KeywordFormatter
    /// </summary>
    public class MyFormatter : KeywordFormatter
    {
        public override string Format(string keyword)
        {
            return "【" + keyword + "】";
        }
    }

    public class TestKeywordFilter
    {
        static int iteration = 10; // 测试次数

        public static void FilterTest1()
        {
            string keyword_string = "SB|法轮功|日你|日你大爷";
            string skipChars = "▇☆";

            KeywordFilterResult result;

            string[] keywords = keyword_string.Split('|');
            KeywordFilter filter1 = new KeywordFilter(keywords);
            KeywordFilter filter2 = new KeywordFilter(keywords, skipChars); // 设置了可跳过的字符
            KeywordFormatter formatter1 = KeywordFormatter.ToIterantStar; //脏字格式化：日你 -> **
            KeywordFormatter formatter2 = new MyFormatter();

            string text = @"我要日你，还要日你大爷，为什么今天一出门就看到个sb在街上练法轮功，其实我都不知道法▇轮☆功到底是神马。";

            Console.WriteLine("原文:");
            WriteResult(text);

            result = filter1.Filter(text, formatter1, KeywordOrder.None, false/* ignoreCase */);
            Console.WriteLine("\r\n不对关键词排序(按初始化时顺序)，区分大小匹配:");
            WriteResult(result.Result);

            result = filter2.Filter(text, formatter1, KeywordOrder.None, true/* ignoreCase */);
            Console.WriteLine("\r\n不对关键词排序(按初始化时顺序)，不区分大小匹配:");
            WriteResult(result.Result);

            result = filter2.Filter(text, formatter1, KeywordOrder.Ascending, true/* ignoreCase */);
            Console.WriteLine("\r\n对关键词Ascending排序(短的先匹配)，不区分大小匹配:");
            WriteResult(result.Result);

            result = filter2.Filter(text, formatter2, KeywordOrder.Descending, true/* ignoreCase */);
            Console.WriteLine("\r\n对关键词Descending排序(长的先匹配)，不区分大小匹配，同时使用自定义formatter:");
            WriteResult(result.Result);

            // 输出所有被过虑的词
            Console.WriteLine("\r\n被过虑的关键词:");
            foreach (string s in result.Keywords)
            {
                Console.Write(s + " | ");
            }
            
            Console.ReadKey();
        }

        public static void FilterTest2()
        {
            string keyword_string = "3p|惨案|64惨剧|64学生运动|64运动|64运动民國|89惨案|89惨剧|89学生运动|89运动|adult|asiangirl|avxiu|av女|awoodong|A片|bbagoori|bbagury|bdsm|binya|bitch|bozy|bunsec|bunsek|byuntae|B样|fa轮|fuck|ＦｕｃΚ|gay|hrichina|jiangzemin|j女|kgirls|kmovie|lihongzhi|MAKELOVE|NND|nude|petish|playbog|playboy|playbozi|pleybog|pleyboy|q奸|realxx|s2x|sex|shit|sorasex|tmb|TMD|tm的|tongxinglian|triangleboy|UltraSurf|unixbox|ustibet|voachinese|webxxx|阿扁|爱姐妹|安全套|骯髒社徻|白立朴|白梦|北大三角地论坛|北京之春|北美自由论坛|屄|逼逼|逼样|笔仙|避孕套|避孕药|婊|婊子|不同政见|藏独|操B|操t|操逼|操比|操蛋|操的货|操她|操你|操你妈|操你媽|操他|操心正|曹长青|曹刚川|草你|插你|常劲|扯淡|陈破空|陈希同|陈小同|陈一咨|陈一谘|陈总统|程铁军|痴汉是犯罪|迟浩田|赤匪|赤化|冲浪赚|丑b|丑逼|臭b|臭逼|臭作|处女膜|春夏自由论坛|打炮|大B|大逼|大波|大啵|大参考|大法|大鸡吧|大纪元|大纪园|大家论坛|大英帝国|大中国论坛|大中华论坛|大众真人真事|戴尔|戴相龙|荡妇|德意志|登辉|邓小平|邓笑贫|迪里夏提|地下教会|地下刊物|丁关根|丁子霖|东北独立|东南西北论谈|东社|东条英机|东突|东土耳其斯坦|东芝|董建华|董贱华|獨立臺灣|独裁|独夫|独立台湾|杜智富|多维|屙民|二b|二逼|二奶|发抡|发伦|发轮|发论|发骚|发正念|法　轮　功|法lun|法功|法兰西|法愣|法抡|法仑|法伦|法轮|法论|法十轮十功|法谪|反攻|反共|反华|反日|梵地冈|方励之|方舟子|斐得勒|费良勇|丰田|风骚|风雨神州|封从德|冯东海|冯素英|佛展千手法|阝月|傅志寰|干你娘|干死你|肛交|肛门|高文谦|高自联|睾丸|戈扬|鸽派|共匪|共狗|共军|狗b|狗j巴|狗操|狗鸡巴|狗卵子|狗娘|狗日|够骚|贯通两极法|龟公|龟头|滚蛋|郭伯雄|郭罗基|郭岩华|哈批|韩东方|汉奸|何德普|河殇|黑手党|红灯区|红色恐怖|宏法|洪传|洪吟|洪哲胜|洪志|胡紧掏|胡锦涛|胡锦滔|胡锦淘|胡景涛|胡总书记|护法|护士24点|花逼|华建敏|华通时事|华夏文摘|华岳时事论坛|皇军|皇君|黄慈萍|黄祸|黄菊|黄色书|黄色图|回民暴动|惠普|鸡8|鸡j|鸡八|鸡巴|鸡奸|鸡毛信文汇|姬胜德|积克馆|基地组织|激情陪聊|激情视频|妓女|贾庆林|贾廷安|贾育台|嫁我网|奸尸|奸淫|建国党|贱b|贱逼|贱比|贱货|贱女人|贱人|溅b|溅逼|江core|江ze民|江八点|江罗|江绵恒|江青|江戏子|江则民|江泽慧|江泽民|江澤民|江贼|江贼民|江猪|江猪媳|江主席|姜春云|姜泽民|将则民|僵贼|僵贼民|疆独|蒋介石|酱猪媳|叫床|揭批书|她tm|她妈的|金尧如|近亲相奸|精虫|精液|精籽|精子|就tm|绝对tm|开放杂志|康柏|靠你妈|可口可乐|恐怖|口交|裤衩大盗|狂操|狂淫|邝锦文|拉丹|拉登|烂逼|烂比|烂货|浪b|浪逼|浪叫|老b|老逼|黎安友|李长春|李大师|李登辉|李登輝|李红痔|李宏志|李洪宽|李洪志|李继耐|李兰菊|李岚清|李瑞环|李少民|李淑娴|李旺阳|李月月鸟|李志绥|李总统|连胜德|联总|廉政大论坛|梁光烈|梁擎墩|两岸三地论坛|两会|廖锡龙|林保华|林长盛|林樵清|林慎立|刘宾深|刘宾雁|刘国凯|刘华清|流亡|龙虎豹|露拉3D|陆委会|吕京花|吕秀莲|呂秀蓮|乱LUN|乱伦|乱性|抡功|伦功|轮大|轮功|轮奸|罗干|罗礼诗|裸聊|裸体|妈B|妈逼|妈了个B|妈了个逼|妈批|马大维|马三家|馬鹿か韓国犬|卖逼|卖比|毛厕洞|毛片|毛泽东|毛主席|美国参考|美国之音|媚日求荣|闷骚|蒙独|蒙古独|蒙古独立|孟加拉|秘穴|密穴|民国|民进党|民意|民意论坛|民运|民阵|民猪|明慧|摩托罗拉|墨索里尼|木子论坛|内裤|奶子|南大自由论坛|倪育贤|你tm|你妈B|你妈逼|你妈的|你媽B|你媽逼|你说我说论坛|你他妈|你丫|牛B|牛X|牛逼|牛比|诺基亚|屁眼|骗钱|破鞋|钱国梁|钱其琛|强J|强暴|强奸|强淫|乔石|轻舟快讯|情妇|去你妈的|群奸|热比娅|热站政论网|人工少女|人民内情真相|人民真实|人民之声论坛|日b|日本人禁止入内|日逼|日你|日你妈|日神话|日死你|三级片|三菱|骚b|骚包|骚逼|骚比|骚的可以|骚货|骚娘们|色b|色逼|色狼|色女|色情|杀你全家|傻|傻B|傻x|傻逼|煞笔|上海帮|社会安全号产生器|社会安全号码产生器|社会安全号码生成|社会安全号码在线|射精|身份号码生成|身份证产生器|身份证号码产生器|身份证号码生成|身份证生成|身份证在线|神通加持法|升天|盛华仁|盛雪|石戈|时代论坛|时事论坛|屎球の粪坑|世纪佳缘|事实独立|手淫|兽奸|双十节|氵去车仑|水扁|司马璐|司徒华|四川独|苏晓康|孙中山|他tm|他妈|他妈的|他奶奶的|他他妈|它他妈|台獨|台独|台盟|台湾|台湾独|台湾狗|台湾建国|台湾青年独立联盟|台湾自由|臺獨|臺独|臺灣|臺灣獨|臺灣狗|臺灣建國|臺灣青年獨立聯盟|臺灣政論|臺灣自由|太子党|汤光中|唐柏桥|滕文生|天安门录影带|天安门事件|天安门屠杀|天安门一代|天怒|舔腚|通奸|同性恋|童屹|统独|屠杀|退出共党|退出中共|退出中国共产党|退党|外交论坛|晚年周恩来|万润南|万维读者论坛|汪岷|王八蛋|王宝森|王策|王超华|王涵万|王若望|王希哲|王冶坪|网络赚钱|网特|网站|微软|尾行|尉健行|慰安妇|魏京生|温家宝|我tm|我操|我日|我他妈|吴百益|吴邦国|吴方城|吴官正|吴弘达|吴宏达|吴学灿|吴学璨|吾尔开希|伍凡|西藏独|吸血莱恩|希特勒|下三滥|下三流|项怀诚|项小吉|小参考|小鸡鸡|小泉|小泉纯一狼|小犬纯一狼|小犬纯夷狼|小犬蠢一狼|小日本|小穴|泄欲|谢长廷|谢选骏|谢中之|新观察论坛|新华内情|新华通论坛|新加坡|新疆独|新生网|新语丝|性爱|性福|性高潮|性交|性快感|性虐|性骚扰|性生活|性诱惑|性欲|熊焱|徐邦秦|徐水良|叙利亚|学潮|血逼|丫的|严家其|严家祺|阎明复|颜射|杨建利|姚月谦|野球拳|夜话紫禁城|夜激情|一夜情|一中一台|以色列|异见人士|异议人士|易丹轩|易志熹|阴j|阴唇|阴道|阴蒂|阴茎|阴毛|阴水|淫荡|淫鬼|淫棍|淫秽|淫贱|淫叫|淫滥|淫乱|淫民|淫魔|淫女|淫娃|淫穴|淫欲|尹庆民|印度神油|英格兰|由喜贵|游具2|诱奸|宇明网|远华案黑幕|杂种|脏逼|曾培炎|曾庆红|张伯笠|张宏堡|张昭富|招妓|找抽吧|找死|赵品潞|赵子阳|赵紫阳|真tm|真鸡吧|真她妈|真她娘|真骚|真善忍|真他妈|真他娘|正义党论坛|政治反对派|支那|指点江山论坛|中国复兴论坛|中国社会进步党|中国社会论坛|中国问题论坛|中国真实内容|中国猪|中华人民共和国|中华人民正邪|中华养生益智功|中华真实报|钟山风雨论坛|周锋锁|周天法|朱容基|朱镕基|猪聋畸|猪媳|拽逼|装b|装x|装逼|自慰|自由民主论坛|自由中国|作爱|做爱";
            string skipChars = " 　═十▇▆■ ▓ 回 □ 〓≡ ╝╚╔ ╗╬ ═ ╓ ╩ ┠ ┨┯ ┷┏ ┓┗ ┛┳⊥『』┌ ┐└ ┘∟「」↑↓→←↘↙♀♂┇┅ ﹉﹊﹍﹎╭ ╮╰ ╯ *^_^* ^*^ ^-^ ^_^ ^（^ ∵∴∥｜ ｜︴﹏﹋﹌（）〔〕 【】〖〗＠：！/_< >`,．。≈{}~ ～()_-『』√ $ @ * & # ※ 卐 々∞Ψ ∪∩∈∏の℡ぁ§∮”〃ミ灬ξ№∑⌒ξζω＊??ㄨ ≮≯ ＋ －×÷＋－±／＝∫∮∝ ∞ ∧∨ ∑ ∏ ∥∠ ≌ ∽ ≤ ≥ ≈＜＞じ ξζω□∮〓※∴ぷ▂▃▅▆█ ∏卐【】△√ ∩¤々♀♂∞①ㄨ≡↘↙▂ ▂ ▃ ▄ ▅ ▆ ▇ █┗┛╰☆╮ ≠ ▂ ▃ ▄ ▅ ";

            // 原始脏字
            List<string> keywords = keyword_string.Split('|').ToList();
            // 从文件读取脏字
            foreach (string line in File.ReadAllLines(EnvironmentUtil.GetFullPath("words.txt")))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    keywords.Add(line);
                }
            }

            KeywordFilterResult result;
            string text = GetText(false);

            Console.WriteLine("\r\n\r\n脏词数量：" + keywords.Count.ToString());
            Console.WriteLine("文章长度：" + text.Length.ToString());
            Console.WriteLine("可跳字符数量：" + skipChars.Length.ToString());

            KeywordFilter filter = new KeywordFilter(keywords, skipChars); // 设置了可跳过的字符

            CodeTimer.Initialize();
            CodeTimer.Time("静态方法", true, iteration, i =>
            {
                result = KeywordFilter.Filter(text, keywords, skipChars, KeywordFormatter.ToIterantStar, KeywordOrder.Descending, true/* ignoreCase */);
            });

            CodeTimer.Time("已初始实例", true, iteration, i =>
            {
                result = filter.Filter(text, KeywordFormatter.ToIterantStar, KeywordOrder.Descending, true/* ignoreCase */);
            });

            Console.ReadKey();
        }

        public static void FilterTest3()
        {
            string keyword_string = "3p|惨案|64惨剧|64学生运动|64运动|64运动民國|89惨案|89惨剧|89学生运动|89运动|adult|asiangirl|avxiu|av女|awoodong|A片|bbagoori|bbagury|bdsm|binya|bitch|bozy|bunsec|bunsek|byuntae|B样|fa轮|fuck|ＦｕｃΚ|gay|hrichina|jiangzemin|j女|kgirls|kmovie|lihongzhi|MAKELOVE|NND|nude|petish|playbog|playboy|playbozi|pleybog|pleyboy|q奸|realxx|s2x|sex|shit|sorasex|tmb|TMD|tm的|tongxinglian|triangleboy|UltraSurf|unixbox|ustibet|voachinese|webxxx|阿扁|爱姐妹|安全套|骯髒社徻|白立朴|白梦|北大三角地论坛|北京之春|北美自由论坛|屄|逼逼|逼样|笔仙|避孕套|避孕药|婊|婊子|不同政见|藏独|操B|操t|操逼|操比|操蛋|操的货|操她|操你|操你妈|操你媽|操他|操心正|曹长青|曹刚川|草你|插你|常劲|扯淡|陈破空|陈希同|陈小同|陈一咨|陈一谘|陈总统|程铁军|痴汉是犯罪|迟浩田|赤匪|赤化|冲浪赚|丑b|丑逼|臭b|臭逼|臭作|处女膜|春夏自由论坛|打炮|大B|大逼|大波|大啵|大参考|大法|大鸡吧|大纪元|大纪园|大家论坛|大英帝国|大中国论坛|大中华论坛|大众真人真事|戴尔|戴相龙|荡妇|德意志|登辉|邓小平|邓笑贫|迪里夏提|地下教会|地下刊物|丁关根|丁子霖|东北独立|东南西北论谈|东社|东条英机|东突|东土耳其斯坦|东芝|董建华|董贱华|獨立臺灣|独裁|独夫|独立台湾|杜智富|多维|屙民|二b|二逼|二奶|发抡|发伦|发轮|发论|发骚|发正念|法　轮　功|法lun|法功|法兰西|法愣|法抡|法仑|法伦|法轮|法论|法十轮十功|法谪|反攻|反共|反华|反日|梵地冈|方励之|方舟子|斐得勒|费良勇|丰田|风骚|风雨神州|封从德|冯东海|冯素英|佛展千手法|阝月|傅志寰|干你娘|干死你|肛交|肛门|高文谦|高自联|睾丸|戈扬|鸽派|共匪|共狗|共军|狗b|狗j巴|狗操|狗鸡巴|狗卵子|狗娘|狗日|够骚|贯通两极法|龟公|龟头|滚蛋|郭伯雄|郭罗基|郭岩华|哈批|韩东方|汉奸|何德普|河殇|黑手党|红灯区|红色恐怖|宏法|洪传|洪吟|洪哲胜|洪志|胡紧掏|胡锦涛|胡锦滔|胡锦淘|胡景涛|胡总书记|护法|护士24点|花逼|华建敏|华通时事|华夏文摘|华岳时事论坛|皇军|皇君|黄慈萍|黄祸|黄菊|黄色书|黄色图|回民暴动|惠普|鸡8|鸡j|鸡八|鸡巴|鸡奸|鸡毛信文汇|姬胜德|积克馆|基地组织|激情陪聊|激情视频|妓女|贾庆林|贾廷安|贾育台|嫁我网|奸尸|奸淫|建国党|贱b|贱逼|贱比|贱货|贱女人|贱人|溅b|溅逼|江core|江ze民|江八点|江罗|江绵恒|江青|江戏子|江则民|江泽慧|江泽民|江澤民|江贼|江贼民|江猪|江猪媳|江主席|姜春云|姜泽民|将则民|僵贼|僵贼民|疆独|蒋介石|酱猪媳|叫床|揭批书|她tm|她妈的|金尧如|近亲相奸|精虫|精液|精籽|精子|就tm|绝对tm|开放杂志|康柏|靠你妈|可口可乐|恐怖|口交|裤衩大盗|狂操|狂淫|邝锦文|拉丹|拉登|烂逼|烂比|烂货|浪b|浪逼|浪叫|老b|老逼|黎安友|李长春|李大师|李登辉|李登輝|李红痔|李宏志|李洪宽|李洪志|李继耐|李兰菊|李岚清|李瑞环|李少民|李淑娴|李旺阳|李月月鸟|李志绥|李总统|连胜德|联总|廉政大论坛|梁光烈|梁擎墩|两岸三地论坛|两会|廖锡龙|林保华|林长盛|林樵清|林慎立|刘宾深|刘宾雁|刘国凯|刘华清|流亡|龙虎豹|露拉3D|陆委会|吕京花|吕秀莲|呂秀蓮|乱LUN|乱伦|乱性|抡功|伦功|轮大|轮功|轮奸|罗干|罗礼诗|裸聊|裸体|妈B|妈逼|妈了个B|妈了个逼|妈批|马大维|马三家|馬鹿か韓国犬|卖逼|卖比|毛厕洞|毛片|毛泽东|毛主席|美国参考|美国之音|媚日求荣|闷骚|蒙独|蒙古独|蒙古独立|孟加拉|秘穴|密穴|民国|民进党|民意|民意论坛|民运|民阵|民猪|明慧|摩托罗拉|墨索里尼|木子论坛|内裤|奶子|南大自由论坛|倪育贤|你tm|你妈B|你妈逼|你妈的|你媽B|你媽逼|你说我说论坛|你他妈|你丫|牛B|牛X|牛逼|牛比|诺基亚|屁眼|骗钱|破鞋|钱国梁|钱其琛|强J|强暴|强奸|强淫|乔石|轻舟快讯|情妇|去你妈的|群奸|热比娅|热站政论网|人工少女|人民内情真相|人民真实|人民之声论坛|日b|日本人禁止入内|日逼|日你|日你妈|日神话|日死你|三级片|三菱|骚b|骚包|骚逼|骚比|骚的可以|骚货|骚娘们|色b|色逼|色狼|色女|色情|杀你全家|傻|傻B|傻x|傻逼|煞笔|上海帮|社会安全号产生器|社会安全号码产生器|社会安全号码生成|社会安全号码在线|射精|身份号码生成|身份证产生器|身份证号码产生器|身份证号码生成|身份证生成|身份证在线|神通加持法|升天|盛华仁|盛雪|石戈|时代论坛|时事论坛|屎球の粪坑|世纪佳缘|事实独立|手淫|兽奸|双十节|氵去车仑|水扁|司马璐|司徒华|四川独|苏晓康|孙中山|他tm|他妈|他妈的|他奶奶的|他他妈|它他妈|台獨|台独|台盟|台湾|台湾独|台湾狗|台湾建国|台湾青年独立联盟|台湾自由|臺獨|臺独|臺灣|臺灣獨|臺灣狗|臺灣建國|臺灣青年獨立聯盟|臺灣政論|臺灣自由|太子党|汤光中|唐柏桥|滕文生|天安门录影带|天安门事件|天安门屠杀|天安门一代|天怒|舔腚|通奸|同性恋|童屹|统独|屠杀|退出共党|退出中共|退出中国共产党|退党|外交论坛|晚年周恩来|万润南|万维读者论坛|汪岷|王八蛋|王宝森|王策|王超华|王涵万|王若望|王希哲|王冶坪|网络赚钱|网特|网站|微软|尾行|尉健行|慰安妇|魏京生|温家宝|我tm|我操|我日|我他妈|吴百益|吴邦国|吴方城|吴官正|吴弘达|吴宏达|吴学灿|吴学璨|吾尔开希|伍凡|西藏独|吸血莱恩|希特勒|下三滥|下三流|项怀诚|项小吉|小参考|小鸡鸡|小泉|小泉纯一狼|小犬纯一狼|小犬纯夷狼|小犬蠢一狼|小日本|小穴|泄欲|谢长廷|谢选骏|谢中之|新观察论坛|新华内情|新华通论坛|新加坡|新疆独|新生网|新语丝|性爱|性福|性高潮|性交|性快感|性虐|性骚扰|性生活|性诱惑|性欲|熊焱|徐邦秦|徐水良|叙利亚|学潮|血逼|丫的|严家其|严家祺|阎明复|颜射|杨建利|姚月谦|野球拳|夜话紫禁城|夜激情|一夜情|一中一台|以色列|异见人士|异议人士|易丹轩|易志熹|阴j|阴唇|阴道|阴蒂|阴茎|阴毛|阴水|淫荡|淫鬼|淫棍|淫秽|淫贱|淫叫|淫滥|淫乱|淫民|淫魔|淫女|淫娃|淫穴|淫欲|尹庆民|印度神油|英格兰|由喜贵|游具2|诱奸|宇明网|远华案黑幕|杂种|脏逼|曾培炎|曾庆红|张伯笠|张宏堡|张昭富|招妓|找抽吧|找死|赵品潞|赵子阳|赵紫阳|真tm|真鸡吧|真她妈|真她娘|真骚|真善忍|真他妈|真他娘|正义党论坛|政治反对派|支那|指点江山论坛|中国复兴论坛|中国社会进步党|中国社会论坛|中国问题论坛|中国真实内容|中国猪|中华人民共和国|中华人民正邪|中华养生益智功|中华真实报|钟山风雨论坛|周锋锁|周天法|朱容基|朱镕基|猪聋畸|猪媳|拽逼|装b|装x|装逼|自慰|自由民主论坛|自由中国|作爱|做爱";
            string skipChars = " 　═十▇▆■ ▓ 回 □ 〓≡ ╝╚╔ ╗╬ ═ ╓ ╩ ┠ ┨┯ ┷┏ ┓┗ ┛┳⊥『』┌ ┐└ ┘∟「」↑↓→←↘↙♀♂┇┅ ﹉﹊﹍﹎╭ ╮╰ ╯ *^_^* ^*^ ^-^ ^_^ ^（^ ∵∴∥｜ ｜︴﹏﹋﹌（）〔〕 【】〖〗＠：！/_< >`,．。≈{}~ ～()_-『』√ $ @ * & # ※ 卐 々∞Ψ ∪∩∈∏の℡ぁ§∮”〃ミ灬ξ№∑⌒ξζω＊??ㄨ ≮≯ ＋ －×÷＋－±／＝∫∮∝ ∞ ∧∨ ∑ ∏ ∥∠ ≌ ∽ ≤ ≥ ≈＜＞じ ξζω□∮〓※∴ぷ▂▃▅▆█ ∏卐【】△√ ∩¤々♀♂∞①ㄨ≡↘↙▂ ▂ ▃ ▄ ▅ ▆ ▇ █┗┛╰☆╮ ≠ ▂ ▃ ▄ ▅ ";
            
            Random rd = new Random(12345678);
            // 原始脏字
            List<string> keywords = keyword_string.Split('|').ToList();
            // 从文件读取脏字
            foreach (string line in File.ReadAllLines(EnvironmentUtil.GetFullPath("words.txt")))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    keywords.Add(line);

                    // 在原来基础上加
                    for (int i = 0; i < 34; ++i)
                    {
                        keywords.Add(keywords + ((char)rd.Next((int)char.MinValue, (int)char.MaxValue)).ToString());
                    }
                }
            }

            KeywordFilterResult result = null;
            string text = GetText(true);//+到10000字

            Console.WriteLine("\r\n\r\n脏词数量：" + keywords.Count.ToString());
            Console.WriteLine("文章长度：" + text.Length.ToString());
            Console.WriteLine("可跳字符数量：" + skipChars.Length.ToString());

            KeywordFilter filter = new KeywordFilter(keywords, skipChars); // 设置了可跳过的字符

            CodeTimer.Initialize();
            CodeTimer.Time("静态方法", true, iteration, i =>
            {
                result = KeywordFilter.Filter(text, keywords, skipChars, KeywordFormatter.ToIterantStar, KeywordOrder.Descending, true/* ignoreCase */);
            });

            CodeTimer.Time("已初始实例", true, iteration, i =>
            {
                result = filter.Filter(text, KeywordFormatter.ToIterantStar, KeywordOrder.Descending, true/* ignoreCase */);
            });

            Console.ReadKey();
            // 输出结果，前1500个字符
            Console.WriteLine(result.Result.Substring(0, 1500));

            Console.ReadKey();
        }

        public static void HighlightTest()
        {
            string keyword_string = "SB|法轮功|日你|日你大爷";
            string skipChars = "▇☆";


            string[] keywords = keyword_string.Split('|');
            KeywordFilter filter1 = new KeywordFilter(keywords);
            KeywordFilter filter2 = new KeywordFilter(keywords, skipChars); // 设置了可跳过的字符
            KeywordFormatter formatter1 = KeywordFormatter.ToIterantStar; //脏字格式化：日你 -> **
            KeywordFormatter formatter2 = new MyFormatter();

            string text = @"我要日你，还要日你大爷，为什么今天一出门就看到个sb在街上练法轮功，其实我都不知道法▇轮☆功到底是神马。";

            string result = Highlighter.Highlight(text, keywords, HighlightFormatter.Html, false);
            Console.WriteLine("\r\nHighlight:");
            WriteResult(result);

            Console.ReadKey();
        }

        private static string GetText(bool batch)
        {
            string text = @"
[利反对派称今日将结束战斗][利反对派称8个月内举行大选] 
云南海南3省区党委主要负责人职务调整
[陈全国任西藏党委书记][秦光荣任云南书记][罗保铭任海南书记]
苹果CEO乔布斯辞职 COO接任 乔布斯语录
[乔布斯仍为董事会主席 辞职信 苹果股价盘后大跌5.39% 热议]
首届政务微博论坛在浙江举行 红灯区|红色恐怖
赵洪祝致贺信[图文直播][视频直播][中国高官开微博成趋势][热议]
铁道部要求年底前无事故 
党员须做公开承诺 胡锦涛会见萨科齐 希望欧方确保中方对欧投资安全 
杭州为接住坠楼小孩“最美妈妈”红灯区|红色恐怖
建城市雕塑(图) 神八将搭载“平民梦想”进太空 
卡扎菲倒 红灯区|红色恐怖
乔布斯辞职了值班编辑：杨达 今日话题百人械斗 医院怎成黑社会
中新网12月14日电中央经济工作会议12月12日至14日在北京举行。会议指出，要坚持房地产调控政策不动摇，促进房价合理回归，加快普通商品住房建设，扩大有效供给，促进房地产市场健康发展。
会议并提出明年经济工作的五项主要任务：第一，继续加强和改善宏观调控，促进经济平稳较快发展。第二，坚持不懈抓好“三农”工作，增强农产品供给保障能力。第三，加快经济结构调整，促进经济自主协调发展。第四，深化重点领域和关键环节改革，提高对外开放水平。第五，大力保障和改善民生，加强和创新社会管理。(中新网财经频道)
------------------------------------------------------------------
这里把 keyword_string 部分脏字都放进来了，模拟文章中有大约10%是需要被过虑的
3p|惨案|64惨剧|64学生运动|64运动|64运动民國|89惨案|89惨剧|89学生运动|89运动|adult|干你娘|干死你|肛交|肛门|高文谦|高自联|睾丸|戈扬|鸽派|共匪|共狗|共军|狗b|狗j巴|狗操|狗鸡巴|asiangirl|avxiu|av女|awoodong|A片|bbagoori|bbagury|bdsm|binya|bitch|bozy猪聋畸|猪媳|拽逼|装b|装x|装逼|自慰|自由民主论坛|自由中国|作爱|做爱
------------------------------------------------------------------";
            if (batch)
            {
                int l = 10; // 改为100就有10万字了
                StringBuilder sb = new StringBuilder(text.Length * l);
                for(int i =0;i<l;++i)
                    sb.Append(text);//+到1万

                text = sb.ToString();
            }

            return text;
        }

        private static void WriteResult(string line)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(line);
            Console.ForegroundColor = color;
        }
    }
}
