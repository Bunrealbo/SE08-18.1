using System;
using ProtoModels;
using UnityEngine;

public class WalletManager
{
	public WalletManager(PlayerModel model, WalletManager.OnSave onSave)
	{
		this.model = model;
		this.onSave = onSave;
	}

	public void ReloadModel(PlayerModel model)
	{
		this.model = model;
	}

	private void DoOnSave()
	{
		if (this.onSave != null)
		{
			this.onSave();
		}
	}

	public void BuyItem(SingleCurrencyPrice price)
	{
		this.BuyItem(price.cost, price.currency);
	}

	public void BuyItem(int price, CurrencyType currencyType)
	{
		if (currencyType == CurrencyType.coins)
		{
			this.BuyItemCoins(price);
			return;
		}
		if (currencyType == CurrencyType.diamonds)
		{
			this.BuyItemDiamonds(price);
			return;
		}
		this.BuyItemDollars(price);
	}

	public void BuyItemDiamonds(int price)
	{
		global::SecureLong s = new global::SecureLong(this.model.secDiamonds, (long)this.model.diamonds);
		s = MathEx.Max(0L, s - (long)price);
		this.model.secDiamonds = s.ToModel();
		this.model.diamonds = Mathf.Max(0, this.model.diamonds - price);
		this.DoOnSave();
	}

	public void BuyItemCoins(int price)
	{
		global::SecureLong s = new global::SecureLong(this.model.secCoins, (long)this.model.coins);
		s = MathEx.Max(0L, s - (long)price);
		this.model.secCoins = s.ToModel();
		this.model.coins = Mathf.Max(0, this.model.coins - price);
		this.DoOnSave();
	}

	public void BuyItemDollars(int price)
	{
		global::SecureLong s = new global::SecureLong(this.model.secGiraffeDollars, (long)this.model.giraffeDollars);
		s = MathEx.Max(0L, s - (long)price);
		this.model.secGiraffeDollars = s.ToModel();
		this.model.giraffeDollars = Mathf.Max(0, this.model.giraffeDollars - price);
		this.DoOnSave();
	}

	public bool CurrencyHasMax(CurrencyType type)
	{
		return true;
	}

	public int MaxCurrencyCount(CurrencyType type)
	{
		if (type == CurrencyType.diamonds)
		{
			return ConfigBase.instance.tokensCap;
		}
		if (type == CurrencyType.coins)
		{
			return ConfigBase.instance.coinsCap;
		}
		if (type == CurrencyType.ggdollars)
		{
			return ConfigBase.instance.ggDollarsCap;
		}
		return 0;
	}

	public void AddCurrency(CurrencyType type, int ammount)
	{
		if (type == CurrencyType.coins)
		{
			global::SecureLong s = new global::SecureLong(this.model.secCoins, (long)this.model.coins);
			this.model.coins += ammount;
			s += (long)ammount;
			if (this.CurrencyHasMax(type))
			{
				this.model.coins = Mathf.Min(this.MaxCurrencyCount(type), this.model.coins);
				s = MathEx.Min((long)this.MaxCurrencyCount(type), s.valueLong);
			}
			this.model.secCoins = s.ToModel();
		}
		else if (type == CurrencyType.diamonds)
		{
			global::SecureLong s2 = new global::SecureLong(this.model.secDiamonds, (long)this.model.diamonds);
			this.model.diamonds += ammount;
			s2 += (long)ammount;
			if (this.CurrencyHasMax(type))
			{
				this.model.diamonds = Mathf.Min(this.MaxCurrencyCount(type), this.model.diamonds);
				s2 = MathEx.Min((long)this.MaxCurrencyCount(type), s2.valueLong);
			}
			this.model.secDiamonds = s2.ToModel();
		}
		else if (type == CurrencyType.ggdollars)
		{
			global::SecureLong s3 = new global::SecureLong(this.model.secGiraffeDollars, (long)this.model.giraffeDollars);
			this.model.giraffeDollars += ammount;
			s3 += (long)ammount;
			if (this.CurrencyHasMax(type))
			{
				this.model.giraffeDollars = Mathf.Min(this.MaxCurrencyCount(type), this.model.giraffeDollars);
				s3 = MathEx.Min((long)this.MaxCurrencyCount(type), s3.valueLong);
			}
			this.model.secGiraffeDollars = s3.ToModel();
		}
		this.DoOnSave();
	}

	public void SetCurrency(CurrencyType type, int ammount)
	{
		if (type == CurrencyType.coins)
		{
			global::SecureLong secureLong = new global::SecureLong(this.model.secCoins, (long)this.model.coins);
			this.model.coins = ammount;
			secureLong = (long)ammount;
			if (this.CurrencyHasMax(type))
			{
				this.model.coins = Mathf.Min(this.MaxCurrencyCount(type), this.model.coins);
				secureLong = MathEx.Min((long)this.MaxCurrencyCount(type), secureLong.valueLong);
			}
			this.model.secCoins = secureLong.ToModel();
		}
		else if (type == CurrencyType.diamonds)
		{
			global::SecureLong secureLong2 = new global::SecureLong(this.model.secDiamonds, (long)this.model.diamonds);
			this.model.diamonds = ammount;
			secureLong2 = (long)ammount;
			if (this.CurrencyHasMax(type))
			{
				this.model.diamonds = Mathf.Min(this.MaxCurrencyCount(type), this.model.diamonds);
				secureLong2 = MathEx.Min((long)this.MaxCurrencyCount(type), secureLong2.valueLong);
			}
			this.model.secDiamonds = secureLong2.ToModel();
		}
		else if (type == CurrencyType.ggdollars)
		{
			global::SecureLong secureLong3 = new global::SecureLong(this.model.secGiraffeDollars, (long)this.model.giraffeDollars);
			this.model.giraffeDollars = ammount;
			secureLong3 = (long)ammount;
			if (this.CurrencyHasMax(type))
			{
				this.model.giraffeDollars = Mathf.Min(this.MaxCurrencyCount(type), this.model.giraffeDollars);
				secureLong3 = MathEx.Min((long)this.MaxCurrencyCount(type), secureLong3.valueLong);
			}
			this.model.secGiraffeDollars = secureLong3.ToModel();
		}
		this.DoOnSave();
	}

	public long CurrencyCount(CurrencyType type)
	{
		if (ConfigBase.instance.secureCurrency)
		{
			global::SecureLong secureLong = 0L;
			if (type == CurrencyType.coins)
			{
				secureLong = new global::SecureLong(this.model.secCoins, (long)this.model.coins);
			}
			else if (type == CurrencyType.diamonds)
			{
				secureLong = new global::SecureLong(this.model.secDiamonds, (long)this.model.diamonds);
			}
			else
			{
				secureLong = new global::SecureLong(this.model.secGiraffeDollars, (long)this.model.giraffeDollars);
			}
			return (long)((int)secureLong.valueLong);
		}
		if (type == CurrencyType.coins)
		{
			return (long)this.model.coins;
		}
		if (type == CurrencyType.diamonds)
		{
			return (long)this.model.diamonds;
		}
		return (long)this.model.giraffeDollars;
	}

	public bool CanBuyItemWithPrice(SingleCurrencyPrice price)
	{
		return this.CanBuyItemWithPrice(price.cost, price.currency);
	}

	public bool CanBuyItemWithPrice(int price, CurrencyType currencyType)
	{
		return this.CurrencyCount(currencyType) >= (long)price;
	}

	private WalletManager.OnSave onSave;

	private PlayerModel model;

	public delegate void OnSave();
}
