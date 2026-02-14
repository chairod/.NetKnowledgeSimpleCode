# Project Lists
### ***HelloConsole***  
 * A. Add ข้อมูลเข้าไปใน List จำนวน 30M (30 ล้านรอบ)   
 เปรียบเทียบที่ใช้เวลาที่ดีที่สุด ในการทำงาน จาก 30M(จำนวน 30 ล้านรอบ)   
  **1.1)** For loop ปกติ   
  **1.2)** For loop แบบใช้ Task.Run(() => ...) แบบนี้ใช้เวลานานมาก เนื่องจากสร้าง Object จำนวน 30M และเป็นภาระของ GC (Gabage collector) เข้าไปเก็บกวาด   
  **1.3)** ConcurrentBage  Parallel.For(....)  
  **1.4)** ConcurrentBage Parallel.For(....) + Chunk ได้เวลาดีกว่า 1.3  
  **1.5)** Exactly dimention + Primitive type + Parallel.For(...)  
  **1.6)** Exactly dimention + Primitive type + Parallel.For(...) + Chunk  ทำเวลาได้ดีที่สุด  
 