using System;

[Serializable]
public class SingleCurrencyPrice
{
	public SingleCurrencyPrice(int cost, CurrencyType currency)
	{
		this.cost = cost;
		this.currency = currency;
	}

	public SingleCurrencyPrice()
	{
	}

	public CurrencyType currency;

	public int cost;
}
