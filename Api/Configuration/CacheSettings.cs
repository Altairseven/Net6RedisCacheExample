using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Configuration;

public class CacheSettings {
    public bool Enabled { get; set; }
    public string? Host { get; set; }
    public string? Port { get; set; }
    public double DefaultLongevity { get; set; } = 300000;


}
