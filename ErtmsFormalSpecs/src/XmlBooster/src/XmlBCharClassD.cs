namespace XmlBooster 
{
	

	public class XmlBCharClassD : XmlBCharClass 
								{


	public override bool validate(string thestring) 
{
	int i = 0;
	while ((i < thestring.Length) &&
	(thestring[i] >= '0') &&
	(thestring[i] <= '9'))
	i++;
	if ((i < 1) || (i == thestring.Length))
	return false;
	if (thestring[i] == ',') return true;
	return false;
}
}

}