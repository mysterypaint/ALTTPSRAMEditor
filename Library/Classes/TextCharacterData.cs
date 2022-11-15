namespace Library.Classes;

public class TextCharacterData
{
    public Dictionary<char, int> EnChar { get; } = new();
    public Dictionary<char, int> JpChar { get; } = new();
    public Dictionary<ushort, char> RawEnChar { get; } = new();
    public Dictionary<ushort, char> RawJpChar { get; } = new();

    public TextCharacterData()
    {
        // Define Font lookup tables
        // English
        EnChar.Add(' ', 0);
        for (var i = 65; i <= 90; i++)
        {
            EnChar.Add((char)i, i - 64);
        }

        for (var i = 97; i <= 122; i++)
        {
            EnChar.Add((char)i, i - 69);
        }

        EnChar.Add('-', 54);
        EnChar.Add('.', 55);
        EnChar.Add(',', 56);
        for (var i = 48; i <= 57; i++)
        {
            EnChar.Add((char)i, i + 11);
        }

        EnChar.Add('!', 69);
        EnChar.Add('?', 70);
        EnChar.Add('(', 71);
        EnChar.Add(')', 72);

        // Japanese
        JpChar.Add('　', 1); // Space

        JpChar.Add('あ', 5); // a
        JpChar.Add('い', 6); // i
        JpChar.Add('う', 7); // u
        JpChar.Add('え', 8); // e
        JpChar.Add('お', 9); // o

        JpChar.Add('か', 10); // ka
        JpChar.Add('き', 11); // ki
        JpChar.Add('く', 12); // ku
        JpChar.Add('け', 13); // ke
        JpChar.Add('こ', 14); // ko

        JpChar.Add('さ', 15); // sa
        JpChar.Add('し', 16); // shi
        JpChar.Add('す', 17); // su
        JpChar.Add('せ', 18); // se
        JpChar.Add('そ', 19); // so

        JpChar.Add('た', 20); // ta
        JpChar.Add('ち', 21); // chi
        JpChar.Add('つ', 22); // tsu
        JpChar.Add('て', 23); // te
        JpChar.Add('と', 24); // to

        JpChar.Add('な', 25); // na
        JpChar.Add('に', 26); // ni
        JpChar.Add('ぬ', 27); // nu
        JpChar.Add('ね', 28); // ne
        JpChar.Add('の', 29); // no

        JpChar.Add('は', 30); // ha
        JpChar.Add('ひ', 31); // hi
        JpChar.Add('ふ', 32); // fu
        JpChar.Add('へ', 33); // he
        JpChar.Add('ほ', 34); // ho

        JpChar.Add('ま', 35); // ma
        JpChar.Add('み', 36); // mi
        JpChar.Add('む', 37); // mu
        JpChar.Add('め', 38); // me
        JpChar.Add('も', 39); // mo

        JpChar.Add('や', 40); // ya
        JpChar.Add('ゆ', 42); // yu
        JpChar.Add('よ', 44); // yo

        JpChar.Add('ら', 45); // ra
        JpChar.Add('り', 46); // ri
        JpChar.Add('る', 47); // ru
        JpChar.Add('れ', 48); // re
        JpChar.Add('ろ', 49); // ro

        JpChar.Add('わ', 50); // wa
        JpChar.Add('を', 51); // wo
        JpChar.Add('ん', 52); // n

        JpChar.Add('ー', 54);

        JpChar.Add('が', 55); // ga
        JpChar.Add('ぎ', 56); // gi
        JpChar.Add('ぐ', 57); // gu
        JpChar.Add('げ', 58); // ge
        JpChar.Add('ご', 59); // go

        JpChar.Add('ざ', 60); // za
        JpChar.Add('じ', 61); // ji
        JpChar.Add('ず', 62); // zu
        JpChar.Add('ぜ', 63); // ze
        JpChar.Add('ぞ', 64); // zo

        JpChar.Add('だ', 65); // da
        JpChar.Add('ぢ', 66); // di
        JpChar.Add('づ', 67); // du
        JpChar.Add('で', 68); // de
        JpChar.Add('ど', 69); // do

        JpChar.Add('ば', 70); // ba
        JpChar.Add('び', 71); // bi
        JpChar.Add('ぶ', 72); // bu
        JpChar.Add('べ', 73); // be
        JpChar.Add('ぼ', 74); // bo

        JpChar.Add('ぱ', 75); // pa
        JpChar.Add('ぴ', 76); // pi
        JpChar.Add('ぷ', 77); // pu
        JpChar.Add('ぺ', 78); // pe
        JpChar.Add('ぽ', 79); // po

        JpChar.Add('ぁ', 80); // ~a
        JpChar.Add('ぃ', 81); // ~i
        JpChar.Add('ぅ', 82); // ~u
        JpChar.Add('ぇ', 83); // ~e
        JpChar.Add('ぉ', 84); // ~o

        JpChar.Add('ゃ', 85); // ~ya
        JpChar.Add('ゅ', 86); // ~yu
        JpChar.Add('ょ', 87); // ~yo

        JpChar.Add('っ', 89); // ~tsu

        // Katakana
        JpChar.Add('ア', 95); // a
        JpChar.Add('イ', 96); // i
        JpChar.Add('ウ', 97); // u
        JpChar.Add('エ', 98); // e
        JpChar.Add('オ', 99); // o

        JpChar.Add('カ', 100); // ka
        JpChar.Add('キ', 101); // ki
        JpChar.Add('ク', 102); // ku
        JpChar.Add('ケ', 103); // ke
        JpChar.Add('コ', 104); // ko

        JpChar.Add('サ', 105); // sa
        JpChar.Add('シ', 106); // shi
        JpChar.Add('ス', 107); // su
        JpChar.Add('セ', 108); // se
        JpChar.Add('ソ', 109); // so

        JpChar.Add('タ', 110); // ta
        JpChar.Add('チ', 111); // chi
        JpChar.Add('ツ', 112); // tsu
        JpChar.Add('テ', 113); // te
        JpChar.Add('ト', 114); // to

        JpChar.Add('ナ', 115); // na
        JpChar.Add('ニ', 116); // ni
        JpChar.Add('ヌ', 117); // nu
        JpChar.Add('ネ', 118); // ne
        JpChar.Add('ノ', 119); // no

        JpChar.Add('ハ', 120); // ha
        JpChar.Add('ヒ', 121); // hi
        JpChar.Add('フ', 122); // fu
        JpChar.Add('ヘ', 123); // he
        JpChar.Add('ホ', 124); // ho

        JpChar.Add('マ', 125); // ma
        JpChar.Add('ミ', 126); // mi
        JpChar.Add('ム', 127); // mu
        JpChar.Add('メ', 128); // me
        JpChar.Add('モ', 129); // mo

        JpChar.Add('ヤ', 130); // ya
        JpChar.Add('ユ', 132); // yu
        JpChar.Add('ヨ', 134); // yo

        JpChar.Add('ラ', 135); // ra
        JpChar.Add('リ', 136); // ri
        JpChar.Add('ル', 137); // ru
        JpChar.Add('レ', 138); // re
        JpChar.Add('ロ', 139); // ro

        JpChar.Add('ワ', 140); // wa
        JpChar.Add('ヲ', 141); // wo
        JpChar.Add('ン', 142); // n

        JpChar.Add('－', 144);

        JpChar.Add('ガ', 145); // ga
        JpChar.Add('ギ', 146); // gi
        JpChar.Add('グ', 147); // gu
        JpChar.Add('ゲ', 148); // ge
        JpChar.Add('ゴ', 149); // go

        JpChar.Add('ザ', 150); // za
        JpChar.Add('ジ', 151); // ji
        JpChar.Add('ズ', 152); // zu
        JpChar.Add('ゼ', 153); // ze
        JpChar.Add('ゾ', 154); // zo

        JpChar.Add('ダ', 155); // da
        JpChar.Add('ヂ', 156); // di
        JpChar.Add('ヅ', 157); // du
        JpChar.Add('デ', 158); // de
        JpChar.Add('ド', 159); // do

        JpChar.Add('バ', 160); // ba
        JpChar.Add('ビ', 161); // bi
        JpChar.Add('ブ', 162); // bu
        JpChar.Add('ベ', 163); // be
        JpChar.Add('ボ', 164); // bo

        JpChar.Add('パ', 165); // pa
        JpChar.Add('ピ', 166); // pi
        JpChar.Add('プ', 167); // pu
        JpChar.Add('ペ', 168); // pe
        JpChar.Add('ポ', 169); // po

        JpChar.Add('ァ', 170); // ~a
        JpChar.Add('ィ', 171); // ~i
        JpChar.Add('ゥ', 172); // ~u
        JpChar.Add('ェ', 173); // ~e
        JpChar.Add('ォ', 174); // ~o

        JpChar.Add('ャ', 175); // ~ya
        JpChar.Add('ュ', 176); // ~yu
        JpChar.Add('ョ', 177); // ~yo

        JpChar.Add('ッ', 179); // ~tsu

        // Romaji
        JpChar.Add('A', 180);
        JpChar.Add('B', 181);
        JpChar.Add('C', 182);
        JpChar.Add('D', 183);
        JpChar.Add('E', 184);
        JpChar.Add('F', 185);
        JpChar.Add('G', 186);
        JpChar.Add('H', 187);
        JpChar.Add('I', 188);
        JpChar.Add('J', 189);
        JpChar.Add('K', 190);
        JpChar.Add('L', 191);
        JpChar.Add('M', 192);
        JpChar.Add('N', 193);
        JpChar.Add('O', 194);
        JpChar.Add('P', 195);
        JpChar.Add('Q', 196);
        JpChar.Add('R', 197);
        JpChar.Add('S', 198);
        JpChar.Add('T', 199);
        JpChar.Add('U', 200);
        JpChar.Add('V', 201);
        JpChar.Add('W', 202);
        JpChar.Add('X', 203);
        JpChar.Add('Y', 204);
        JpChar.Add('Z', 205);
        JpChar.Add('-', 206);
        JpChar.Add('~', 207);

        // Raw in-game hex chars
        // English
        RawEnChar.Add(0xA9, ' ');
        RawEnChar.Add(0x00, 'A');
        RawEnChar.Add(0x01, 'B');
        RawEnChar.Add(0x02, 'C');
        RawEnChar.Add(0x03, 'D');
        RawEnChar.Add(0x04, 'E');
        RawEnChar.Add(0x05, 'F');
        RawEnChar.Add(0x06, 'G');
        RawEnChar.Add(0x07, 'H');
        RawEnChar.Add(0x08, 'I');
        RawEnChar.Add(0x09, 'J');
        RawEnChar.Add(0x0A, 'K');
        RawEnChar.Add(0x0B, 'L');
        RawEnChar.Add(0x0C, 'M');
        RawEnChar.Add(0x0D, 'N');
        RawEnChar.Add(0x0E, 'O');
        RawEnChar.Add(0x0F, 'P');
        RawEnChar.Add(0x10, '%'); // Glitched ? char, use 0x6F instead
        RawEnChar.Add(0x20, 'Q');
        RawEnChar.Add(0x21, 'R');
        RawEnChar.Add(0x22, 'S');
        RawEnChar.Add(0x23, 'T');
        RawEnChar.Add(0x24, 'U');
        RawEnChar.Add(0x25, 'V');
        RawEnChar.Add(0x26, 'W');
        RawEnChar.Add(0x27, 'X');
        RawEnChar.Add(0x28, 'Y');
        RawEnChar.Add(0x29, 'Z');
        RawEnChar.Add(0x2A, 'a');
        RawEnChar.Add(0x2B, 'b');
        RawEnChar.Add(0x2C, 'c');
        RawEnChar.Add(0x2D, 'd');
        RawEnChar.Add(0x2E, 'e');
        RawEnChar.Add(0x2F, 'f');
        RawEnChar.Add(0x40, 'g');
        RawEnChar.Add(0x41, 'h');
        RawEnChar.Add(0xC0, 'i');
        RawEnChar.Add(0x43, 'j');
        RawEnChar.Add(0x44, 'k');
        RawEnChar.Add(0xAF, 'l');
        RawEnChar.Add(0x46, 'm');
        RawEnChar.Add(0x47, 'n');
        RawEnChar.Add(0x48, 'o');
        RawEnChar.Add(0x49, 'p');
        RawEnChar.Add(0x4A, 'q');
        RawEnChar.Add(0x4B, 'r');
        RawEnChar.Add(0x4C, 's');
        RawEnChar.Add(0x4D, 't');
        RawEnChar.Add(0x4E, 'u');
        RawEnChar.Add(0x4F, 'v');
        RawEnChar.Add(0x60, 'w');
        RawEnChar.Add(0x61, 'x');
        RawEnChar.Add(0x62, 'y');
        RawEnChar.Add(0x63, 'z');
        RawEnChar.Add(0xE6, '0');
        RawEnChar.Add(0xE7, '1');
        RawEnChar.Add(0xE8, '2');
        RawEnChar.Add(0xE9, '3');
        RawEnChar.Add(0xEA, '4');
        RawEnChar.Add(0xEB, '5');
        RawEnChar.Add(0xEC, '6');
        RawEnChar.Add(0xED, '7');
        RawEnChar.Add(0xEE, '8');
        RawEnChar.Add(0xEF, '9');
        RawEnChar.Add(0xC1, '!');
        RawEnChar.Add(0x6F, '?');
        RawEnChar.Add(0x80, '-');
        RawEnChar.Add(0x81, '.');
        RawEnChar.Add(0x82, ',');
        RawEnChar.Add(0x85, '(');
        RawEnChar.Add(0x86, ')');

        // Japanese
        // Hiragana
        RawJpChar.Add(0x18C, '　');
        RawJpChar.Add(0x0, 'あ'); // a
        RawJpChar.Add(0x1, 'い'); // i
        RawJpChar.Add(0x2, 'う'); // u
        RawJpChar.Add(0x3, 'え'); // e
        RawJpChar.Add(0x4, 'お'); // o

        RawJpChar.Add(0x8, 'か'); // ka
        RawJpChar.Add(0x9, 'き'); // ki
        RawJpChar.Add(0xA, 'く'); // ku
        RawJpChar.Add(0xB, 'け'); // ke
        RawJpChar.Add(0xC, 'こ'); // ko

        RawJpChar.Add(0x20, 'さ'); // sa
        RawJpChar.Add(0x21, 'し'); // shi
        RawJpChar.Add(0x22, 'す'); // su
        RawJpChar.Add(0x23, 'せ'); // se
        RawJpChar.Add(0x24, 'そ'); // so

        RawJpChar.Add(0x28, 'た'); // ta
        RawJpChar.Add(0x29, 'ち'); // chi
        RawJpChar.Add(0x2A, 'つ'); // tsu
        RawJpChar.Add(0x2B, 'て'); // te
        RawJpChar.Add(0x2C, 'と'); // to

        RawJpChar.Add(0x40, 'な'); // na
        RawJpChar.Add(0x41, 'に'); // ni
        RawJpChar.Add(0x42, 'ぬ'); // nu
        RawJpChar.Add(0x43, 'ね'); // ne
        RawJpChar.Add(0x44, 'の'); // no

        RawJpChar.Add(0x48, 'は'); // ha
        RawJpChar.Add(0x49, 'ひ'); // hi
        RawJpChar.Add(0x4A, 'ふ'); // fu
        RawJpChar.Add(0x4B, 'へ'); // he
        RawJpChar.Add(0x4C, 'ほ'); // ho

        RawJpChar.Add(0x60, 'ま'); // ma
        RawJpChar.Add(0x61, 'み'); // mi
        RawJpChar.Add(0x62, 'む'); // mu
        RawJpChar.Add(0x63, 'め'); // me
        RawJpChar.Add(0x64, 'も'); // mo

        RawJpChar.Add(0x5, 'や'); // ya
        RawJpChar.Add(0x6, 'ゆ'); // yu
        RawJpChar.Add(0x7, 'よ'); // yo

        RawJpChar.Add(0x68, 'ら'); // ra
        RawJpChar.Add(0x69, 'り'); // ri
        RawJpChar.Add(0x6A, 'る'); // ru
        RawJpChar.Add(0x6B, 'れ'); // re
        RawJpChar.Add(0x6C, 'ろ'); // ro

        RawJpChar.Add(0xD, 'わ'); // wa
        RawJpChar.Add(0xE, 'を'); // wo
        RawJpChar.Add(0xF, 'ん'); // n

        RawJpChar.Add(0x189, 'ー');

        RawJpChar.Add(0x25, 'が'); // ga
        RawJpChar.Add(0x26, 'ぎ'); // gi
        RawJpChar.Add(0x27, 'ぐ'); // gu
        RawJpChar.Add(0x2D, 'げ'); // ge
        RawJpChar.Add(0x2E, 'ご'); // go

        RawJpChar.Add(0x2F, 'ざ'); // za
        RawJpChar.Add(0x45, 'じ'); // ji
        RawJpChar.Add(0x46, 'ず'); // zu
        RawJpChar.Add(0x47, 'ぜ'); // ze
        RawJpChar.Add(0x4D, 'ぞ'); // zo

        RawJpChar.Add(0x4E, 'だ'); // da
        RawJpChar.Add(0x4F, 'ぢ'); // di
        RawJpChar.Add(0x65, 'づ'); // du
        RawJpChar.Add(0x66, 'で'); // de
        RawJpChar.Add(0x67, 'ど'); // do

        RawJpChar.Add(0x6D, 'ば'); // ba
        RawJpChar.Add(0x6E, 'び'); // bi
        RawJpChar.Add(0x6F, 'ぶ'); // bu
        RawJpChar.Add(0x80, 'べ'); // be
        RawJpChar.Add(0x81, 'ぼ'); // bo

        RawJpChar.Add(0x82, 'ぱ'); // pa
        RawJpChar.Add(0x83, 'ぴ'); // pi
        RawJpChar.Add(0x84, 'ぷ'); // pu
        RawJpChar.Add(0x85, 'ぺ'); // pe
        RawJpChar.Add(0x86, 'ぽ'); // po

        RawJpChar.Add(0x8B, 'ぁ'); // ~a
        RawJpChar.Add(0x8C, 'ぃ'); // ~i
        RawJpChar.Add(0x8D, 'ぅ'); // ~u
        RawJpChar.Add(0x8E, 'ぇ'); // ~e
        RawJpChar.Add(0x8F, 'ぉ'); // ~o

        RawJpChar.Add(0x87, 'ゃ'); // ~ya
        RawJpChar.Add(0x88, 'ゅ'); // ~yu
        RawJpChar.Add(0x89, 'ょ'); // ~yo

        RawJpChar.Add(0x8A, 'っ'); // ~tsu

        // Katakana
        RawJpChar.Add(0xA0, 'ア'); // a
        RawJpChar.Add(0xA1, 'イ'); // i
        RawJpChar.Add(0xA2, 'ウ'); // u
        RawJpChar.Add(0xA3, 'エ'); // e
        RawJpChar.Add(0xA4, 'オ'); // o

        RawJpChar.Add(0xA8, 'カ'); // ka
        RawJpChar.Add(0xA9, 'キ'); // ki
        RawJpChar.Add(0xAA, 'ク'); // ku
        RawJpChar.Add(0xAB, 'ケ'); // ke
        RawJpChar.Add(0xAC, 'コ'); // ko

        RawJpChar.Add(0xC0, 'サ'); // sa
        RawJpChar.Add(0xC1, 'シ'); // shi
        RawJpChar.Add(0xC2, 'ス'); // su
        RawJpChar.Add(0xC3, 'セ'); // se
        RawJpChar.Add(0xC4, 'ソ'); // so

        RawJpChar.Add(0xC8, 'タ'); // ta
        RawJpChar.Add(0xC9, 'チ'); // chi
        RawJpChar.Add(0xCA, 'ツ'); // tsu
        RawJpChar.Add(0xCB, 'テ'); // te
        RawJpChar.Add(0xCC, 'ト'); // to

        RawJpChar.Add(0xE0, 'ナ'); // na
        RawJpChar.Add(0xE1, 'ニ'); // ni
        RawJpChar.Add(0xE2, 'ヌ'); // nu
        RawJpChar.Add(0xE3, 'ネ'); // ne
        RawJpChar.Add(0xE4, 'ノ'); // no

        RawJpChar.Add(0xE8, 'ハ'); // ha
        RawJpChar.Add(0xE9, 'ヒ'); // hi
        RawJpChar.Add(0xEA, 'フ'); // fu
        RawJpChar.Add(0xEB, 'ヘ'); // he
        RawJpChar.Add(0xEC, 'ホ'); // ho

        RawJpChar.Add(0x100, 'マ'); // ma
        RawJpChar.Add(0x101, 'ミ'); // mi
        RawJpChar.Add(0x102, 'ム'); // mu
        RawJpChar.Add(0x103, 'メ'); // me
        RawJpChar.Add(0x104, 'モ'); // mo

        RawJpChar.Add(0xA5, 'ヤ'); // ya
        RawJpChar.Add(0xA6, 'ユ'); // yu
        RawJpChar.Add(0xA7, 'ヨ'); // yo

        RawJpChar.Add(0x108, 'ラ'); // ra
        RawJpChar.Add(0x109, 'リ'); // ri
        RawJpChar.Add(0x10A, 'ル'); // ru
        RawJpChar.Add(0x10B, 'レ'); // re
        RawJpChar.Add(0x10C, 'ロ'); // ro

        RawJpChar.Add(0xAD, 'ワ'); // wa
        RawJpChar.Add(0xAE, 'ヲ'); // wo
        RawJpChar.Add(0xAF, 'ン'); // n

        //rawJPChar.Add(0x189, '－'); <- Exactly the same as hiragana; accounted for in the name-reading code

        RawJpChar.Add(0xC5, 'ガ'); // ga
        RawJpChar.Add(0xC6, 'ギ'); // gi
        RawJpChar.Add(0xC7, 'グ'); // gu
        RawJpChar.Add(0xCD, 'ゲ'); // ge
        RawJpChar.Add(0xCE, 'ゴ'); // go

        RawJpChar.Add(0xCF, 'ザ'); // za
        RawJpChar.Add(0xE5, 'ジ'); // ji
        RawJpChar.Add(0xE6, 'ズ'); // zu
        RawJpChar.Add(0xE7, 'ゼ'); // ze
        RawJpChar.Add(0xED, 'ゾ'); // zo

        RawJpChar.Add(0xEE, 'ダ'); // da
        RawJpChar.Add(0xEF, 'ヂ'); // di
        RawJpChar.Add(0x105, 'ヅ'); // du
        RawJpChar.Add(0x106, 'デ'); // de
        RawJpChar.Add(0x107, 'ド'); // do

        RawJpChar.Add(0x10D, 'バ'); // ba
        RawJpChar.Add(0x10E, 'ビ'); // bi
        RawJpChar.Add(0x10F, 'ブ'); // bu
        RawJpChar.Add(0x120, 'ベ'); // be
        RawJpChar.Add(0x121, 'ボ'); // bo

        RawJpChar.Add(0x122, 'パ'); // pa
        RawJpChar.Add(0x123, 'ピ'); // pi
        RawJpChar.Add(0x124, 'プ'); // pu
        RawJpChar.Add(0x125, 'ペ'); // pe
        RawJpChar.Add(0x126, 'ポ'); // po

        RawJpChar.Add(0x12B, 'ァ'); // ~a
        RawJpChar.Add(0x12C, 'ィ'); // ~i
        RawJpChar.Add(0x12D, 'ゥ'); // ~u
        RawJpChar.Add(0x12E, 'ェ'); // ~e
        RawJpChar.Add(0x12F, 'ォ'); // ~o

        RawJpChar.Add(0x127, 'ャ'); // ~ya
        RawJpChar.Add(0x128, 'ュ'); // ~yu
        RawJpChar.Add(0x129, 'ョ'); // ~yo

        RawJpChar.Add(0x12A, 'ッ'); // ~tsu

        // Romaji
        RawJpChar.Add(0x014A, 'A');
        RawJpChar.Add(0x014B, 'B');
        RawJpChar.Add(0x014C, 'C');
        RawJpChar.Add(0x014D, 'D');
        RawJpChar.Add(0x014E, 'E');
        RawJpChar.Add(0x014F, 'F');
        RawJpChar.Add(0x0160, 'G');
        RawJpChar.Add(0x0161, 'H');
        RawJpChar.Add(0x0162, 'I');
        RawJpChar.Add(0x0163, 'J');
        RawJpChar.Add(0x0164, 'K');
        RawJpChar.Add(0x0165, 'L');
        RawJpChar.Add(0x0166, 'M');
        RawJpChar.Add(0x0167, 'N');
        RawJpChar.Add(0x0168, 'O');
        RawJpChar.Add(0x0169, 'P');
        RawJpChar.Add(0x016A, 'Q');
        RawJpChar.Add(0x016B, 'R');
        RawJpChar.Add(0x016C, 'S');
        RawJpChar.Add(0x016D, 'T');
        RawJpChar.Add(0x016E, 'U');
        RawJpChar.Add(0x016F, 'V');
        RawJpChar.Add(0x0180, 'W');
        RawJpChar.Add(0x0181, 'X');
        RawJpChar.Add(0x0182, 'Y');
        RawJpChar.Add(0x0183, 'Z');
        //rawJPChar.Add(0x0189, '-'); <- Exactly the same as hiragana; accounted for in the name-reading code
        RawJpChar.Add(0x018E, '~');
    }
}
