using System;
using ProtoModels;
using UnityEngine;

public struct SecureLong
{
	public long valueLong
	{
		get
		{
			return this.Evaluate();
		}
	}

	public SecureLong(ProtoModels.SecureLong model, long defaultValue)
	{
		if (model == null || !model.isAssigned)
		{
			this.key1 = 0L;
			this.key2 = 0L;
			this.key3 = 0L;
			this.key4 = 0L;
			this.value1 = 0L;
			this.value2 = 0L;
			this.value3 = 0L;
			this.value4 = 0L;
			this.value5 = 0L;
			this.value6 = 0L;
			this.value7 = 0L;
			this.value8 = 0L;
			this.value9 = 0L;
			this.value10 = 0L;
			this.isAssigned = false;
			this.isTamperedWith = false;
			this.Assign(defaultValue);
			return;
		}
		this.key1 = model.key1;
		this.key2 = model.key2;
		this.key3 = model.key3;
		this.key4 = model.key4;
		this.value1 = model.value1;
		this.value2 = model.value2;
		this.value3 = model.value3;
		this.value4 = model.value4;
		this.value5 = model.value5;
		this.value6 = model.value6;
		this.value7 = model.value7;
		this.value8 = model.value8;
		this.value9 = model.value9;
		this.value10 = model.value10;
		this.isAssigned = model.isAssigned;
		this.isTamperedWith = model.isTamperedWith;
	}

	public ProtoModels.SecureLong ToModel()
	{
		return new ProtoModels.SecureLong
		{
			key1 = this.key1,
			key2 = this.key2,
			key3 = this.key3,
			key4 = this.key4,
			value1 = this.value1,
			value2 = this.value2,
			value3 = this.value3,
			value4 = this.value4,
			value5 = this.value5,
			value6 = this.value6,
			value7 = this.value7,
			value8 = this.value8,
			value9 = this.value9,
			value10 = this.value10,
			isAssigned = this.isAssigned,
			isTamperedWith = this.isTamperedWith
		};
	}

	public void ShuffleKeys()
	{
		this.key1 = (long)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		this.key2 = (long)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		this.key3 = (long)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		this.key4 = (long)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
	}

	private long Encrypt1(long value)
	{
		value += 199L;
		return value;
	}

	private long Decrypt1(long value)
	{
		value -= 199L;
		return value;
	}

	private long Encrypt2(long value)
	{
		value = (value ^ this.key1) + 88L;
		return value;
	}

	private long Decrypt2(long value)
	{
		value = (value - 88L ^ this.key1);
		return value;
	}

	private long Encrypt3(long value)
	{
		value = (value + 177L ^ this.key2);
		return value;
	}

	private long Decrypt3(long value)
	{
		value = (value ^ this.key2) - 177L;
		return value;
	}

	private long Encrypt4(long value)
	{
		value = (value ^ this.key2) + (381L ^ this.key3);
		return value;
	}

	private long Decrypt4(long value)
	{
		value = (value - (381L ^ this.key3) ^ this.key2);
		return value;
	}

	private long Encrypt5(long value)
	{
		return (long)UnityEngine.Random.Range(-100, 100);
	}

	private long Encrypt6(long value)
	{
		value -= 300L;
		return value;
	}

	private long Decrypt6(long value)
	{
		value += 300L;
		return value;
	}

	private long Encrypt7(long value)
	{
		value = (value + 261L ^ this.key4);
		return value;
	}

	private long Decrypt7(long value)
	{
		value = (value ^ this.key4) - 261L;
		return value;
	}

	private long Encrypt8(long value)
	{
		return (long)UnityEngine.Random.Range(-100, 100);
	}

	private long Encrypt9(long value)
	{
		return (long)UnityEngine.Random.Range(-100, 100);
	}

	private long Encrypt10(long value)
	{
		return (long)UnityEngine.Random.Range(-100, 100);
	}

	private void Assign(long value)
	{
		this.ShuffleKeys();
		this.isAssigned = true;
		this.value1 = this.Encrypt1(value);
		this.value2 = this.Encrypt2(value);
		this.value3 = this.Encrypt3(value);
		this.value4 = this.Encrypt4(value);
		this.value5 = this.Encrypt5(value);
		this.value6 = this.Encrypt6(value);
		this.value7 = this.Encrypt7(value);
		this.value8 = this.Encrypt8(value);
		this.value9 = this.Encrypt9(value);
		this.value10 = this.Encrypt10(value);
	}

	private long Evaluate()
	{
		long num = this.Decrypt1(this.value1);
		long num2 = num;
		num = Math.Min(num, this.Decrypt2(this.value2));
		if (num2 != num)
		{
			this.isTamperedWith = true;
		}
		long num3 = num;
		num = Math.Min(num, this.Decrypt3(this.value3));
		if (num3 != num)
		{
			this.isTamperedWith = true;
		}
		long num4 = num;
		num = Math.Min(num, this.Decrypt4(this.value4));
		if (num4 != num)
		{
			this.isTamperedWith = true;
		}
		long num5 = num;
		num = Math.Min(num, this.Decrypt6(this.value6));
		if (num5 != num)
		{
			this.isTamperedWith = true;
		}
		long num6 = num;
		num = Math.Min(num, this.Decrypt7(this.value7));
		if (num6 != num)
		{
			this.isTamperedWith = true;
		}
		return num;
	}

	public static long operator +(global::SecureLong s, long i)
	{
		return s.Evaluate() + i;
	}

	public static long operator -(global::SecureLong s, long i)
	{
		return s.Evaluate() - i;
	}

	public static implicit operator global::SecureLong(long value)
	{
		global::SecureLong result = default(global::SecureLong);
		result.Assign(value);
		return result;
	}

	private long key1;

	private long key2;

	private long key3;

	private long key4;

	private long value1;

	private long value2;

	private long value3;

	private long value4;

	private long value5;

	private long value6;

	private long value7;

	private long value8;

	private long value9;

	private long value10;

	public bool isAssigned;

	public bool isTamperedWith;
}
