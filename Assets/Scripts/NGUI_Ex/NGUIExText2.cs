static public partial class NGUIExText
{
	private static BMSymbol MatchCustomSymbol (string text, int index, int textLength)
	{
		BMSymbol customSymbol = null;

		if (index >= textLength)
			return customSymbol;

		if (text[index] == '#') {
			int endIndex = index + 1;
			while (endIndex < textLength) {
				if (text[endIndex] == '#')
					break;
				else
					endIndex++;
			}
			if (endIndex == textLength)
				customSymbol = null;
			else {
				int startIndex = index + 1;
				customSymbol = GetCustomSymbol(text.Substring(startIndex, endIndex - startIndex));
			}
		}
		return customSymbol;
	}

	public static BMSymbol GetCustomSymbol(string text, ref int index)
	{
		BMSymbol customSymbol = MatchCustomSymbol(text, index, text.Length);
        if(customSymbol != null)
            index = index + customSymbol.length + 2 - 1;
        return customSymbol;
    }

	public static BMSymbol GetCustomSymbol(string spriteName)
    {
        if (string.IsNullOrEmpty(spriteName))
            return null;

        BMSymbol symbol = null;
        if (faceAtlas != null)
        {
            symbol = new BMSymbol();
            symbol.spriteName = spriteName;
            bool flag = symbol.Validate(faceAtlas);
            if (!flag)
                symbol = null;
            else
                symbol.sequence = spriteName;
        }
        return symbol;
    }
}
