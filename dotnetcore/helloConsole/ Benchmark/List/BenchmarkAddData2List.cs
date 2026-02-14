using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace helloConsole.Benchmark.List
{
    public static class BenchmarkAddData2List
    {
        
        /// <summary>
        /// ทดสอบ วิธีการ เพิ่มข้อมูลเข้าไปใน List จำนวน 30M (30 ล้านรายการ)
        /// <para>สิ่งที่ใช้ทดสอบ ConcurrentBage, Exactly Type (กำหนดประเภทข้อมูลที่ชัดเจน), Exactly dimention of array<para>
        /// บทสรุป จากผลลัพธ์ที่ได้:
        ///     เหมือนถ้าเราทราบ จำนวน dimension of array ที่แท้จริง และ ทราบ Data Type ที่ต้องเก็บจริงๆ ไม่ใช้ Object เพราะตามที่คุณบอก ต้องเสีย Overhead ในการ Convert Data => Object
        ///     ผลลัพธ์ที่ได้จากจำนวนตัวอย่าง 30M จะเร็วกว่า
        ///     และ Improvement performance ได้ดียิ่งขึ้นหากเราทำ Chunk เพื่อแบ่งงาน (ใน Benchmark แบ่ง Chunk ล่ะ 10,000)
        /// </summary>
        public static void Try2AddListBenchMark()
        {

            ConcurrentBag<int> lstObj = new();
            var simpleTestLoop = 30_000_000; // 30M loop
            const int rndMinValue = 1;
            const int rndMaxValue = 100000000;
            var chunkCapacity = 10_000;
            var chunkSize = Convert.ToInt32(Math.Ceiling(simpleTestLoop / Convert.ToDouble(chunkCapacity)));

            Console.WriteLine($"Add item into list simple: {simpleTestLoop:N0} loop");


            Stopwatch sw = Stopwatch.StartNew();
            List<Task> workers = new();



            // #1 - Normal for
            lstObj.Clear();
            sw.Restart();
            Random rnd = new();
            do
            {
                var value = rnd.Next(rndMinValue, rndMaxValue);
                lstObj.Add(value);
            } while (lstObj.Count <= simpleTestLoop);
            sw.Stop();
            Console.WriteLine($"#1. Normal for - Operation Time: {sw.Elapsed.TotalSeconds:N5} second(s)");


            // #2 Task.run()
            // Note: การเขียนแบบนี้ ใช้เวลาในการทำงาน ~26 วินาที++ 
            // ข้อเสียเกิดจาก มีการสร้าง งานจำนวนมาก (30M) รายการ ถึงแม้จะเป็นการรันทีล่ะ 5 Task  ให้จบก่อนก็ตาม
            // แต่การทำงานจริงๆที่เกิดขึ้น จะเกิด Object จำนวน  30M ที่ GC (Gabage collector) ที่ต้องไปไล่เก็บกวาด
            // Flow ของโค้ดจะเป็น:
            // Create Task -> Add Thread pool -> Main thread Wait & Hey!! Walkup 5 thread -> 5 Thread wokring and done


            // lstObj.Clear();
            // sw.Restart();
            // do
            // {
            //     var value = rnd.Next(rndMinValue, rndMaxValue);
            //     workers.Add(Task.Run(() => lstObj.Add(value)));
            //     if (workers.Count > 5)
            //     {
            //         Task.WaitAll(workers);
            //         workers.Clear();
            //     }
            // } while (lstObj.Count <= simpleTestLoop);
            // sw.Stop();
            // Console.WriteLine($"#2. Task.Run() - Operation Time: {sw.Elapsed.TotalSeconds:N5} second(s)");


            // #3 Parallel.For - ConcurrentBage
            sw.Restart();
            lstObj.Clear();
            Parallel.For(0, simpleTestLoop, _ =>
            {
                var localRnd = Random.Shared;
                var value = localRnd.Next(rndMinValue, rndMaxValue);
                lstObj.Add(value);
            });
            sw.Stop();
            Console.WriteLine($"#3. Parallel.For (ConcurrentBage) - Operation Time: {sw.Elapsed.TotalSeconds:N5} Second(s)");


            // #4 - Parall.For - ConcurrentBage + Seperate by chunkCapacity 10,000
            sw.Restart();
            lstObj.Clear();
            Parallel.For(1, chunkSize + 1, _ =>
            {
                var localRnd = Random.Shared;
                for (var IX = 0; IX < chunkCapacity; IX++)
                {
                    var intVal = localRnd.Next(rndMinValue, rndMaxValue);
                    lstObj.Add(intVal);
                }
            });
            sw.Stop();
            Console.WriteLine($"#4 Parallel.For (ConcurrentBage + chunkCapacity {chunkCapacity:N0}) - Operation Time: {sw.Elapsed.TotalSeconds:N5} Second(s)");


            // #5 Parallel.For - Array + Exactly type
            sw.Restart();
            int[] arParallels = new int[simpleTestLoop];
            Parallel.For(1, simpleTestLoop, ix =>
            {
                var localRnd = Random.Shared;
                var value = localRnd.Next(rndMinValue, rndMaxValue);
                arParallels[ix] = value;
            });
            sw.Stop();
            Console.WriteLine($"#5. Paral.for (Array + Exactly data Type) - Operation Time: {sw.Elapsed.TotalSeconds:N5} Second(s)");



            // #6 Paral.for - Array + Exactly data Type + chunkCapacity 
            arParallels = new int[simpleTestLoop];
            sw.Restart();
            int[] chunkOperated = new int[chunkSize];
            Parallel.For(1, chunkSize + 1, chunkIX =>
            {
                var localRnd = Random.Shared;

                chunkOperated[chunkIX - 1] = chunkIX;

                var startIX = chunkIX * chunkCapacity - chunkCapacity;

                // chunkIX * chunkCapacity จะได้เท่ากับ chunkCapacity และนำมาลบกับ chunkCapacity จะได้เท่ากับ 0
                // -1 เนื่องจาก จุดเริ่มของ StartIX = 0
                var endIX = startIX + chunkCapacity - 1; 
                for (var IX = startIX; IX <= endIX; IX++)
                {
                    var value = localRnd.Next(rndMinValue, rndMaxValue);
                    arParallels[IX] = value;
                }
            });
            sw.Stop();
            Console.WriteLine($"#6. Paral.for (Array + Exactly data Type + chunkCapacity {chunkCapacity:N0}) - Operation Time: {sw.Elapsed.TotalSeconds:N5} Second(s)");
            Console.WriteLine($"Value in array - MIN: {arParallels.Min():N0}, MAX: {arParallels.Max():N0}");
            Console.WriteLine($"Operated Chunk {chunkSize:N0} - MIN: {chunkOperated.Min():N0}, MAX: {chunkOperated.Max():N0}");


        }
    }
}