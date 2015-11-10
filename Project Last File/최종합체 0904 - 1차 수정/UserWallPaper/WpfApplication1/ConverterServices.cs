using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.gmail.nishantsinhaindia.DocumentConverter;

namespace UserWallPaper
{
    // PPT, WORD, EXECEL 변환하기위한 클래스
    static public class ConverterServices
    {
        static public short convert(string inputFile, string outputFile)
        {
            short conversionResult = -1;

            ContentType fileExtn = ContentType.UNKNOWN;
            conversionResult = Util.getFileExtension(inputFile, out fileExtn);
            if (conversionResult == 0)
            {
                IDocumentConverterFactory factory = new DocumentConverterFactory();

                IConverter converter = factory.getConverter(ConversionType.Doc2Pdf);
                if (converter != null)
                    conversionResult = converter.convert(inputFile, outputFile, fileExtn);
            }
            return conversionResult;
        }
    }
}
