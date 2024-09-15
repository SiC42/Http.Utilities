internal readonly record struct QueryParameter
{
  private readonly string queryParam;

  public QueryParameter(string queryParam)
  {
    var split = queryParam.IndexOf("=");
    if (split is -1)
    {
      Key = queryParam;
    }
    else
    {
      Key = queryParam[0..split];
      Value = queryParam[Math.Min(split + 1, queryParam.Length - 1)..^1];
    }

    this.queryParam = queryParam;
  }

  public QueryParameter(string key, string value)
  {
    Key = key;
    Value = value;
    queryParam = $"{key}={value}";
  }

  public string Key { get; }
  public string? Value { get; }

  public override string ToString()
  {
    return queryParam;
  }
}