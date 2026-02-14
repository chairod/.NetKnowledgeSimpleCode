using System.Collections.Concurrent;
using System.Diagnostics;
using helloConsole.Benchmark.List;
using helloConsole.Module.List;
using Newtonsoft.Json;



Console.WriteLine("Hey!!, This main method of console program");



// Benchmark add simple 30M item into list
BenchmarkAddData2List.Try2AddListBenchMark();
