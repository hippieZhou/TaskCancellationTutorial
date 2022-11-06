<div>

# Asynchronous Programming In C#

</div>

## IO Bound or CPU Bound

### IO 模型

### CPU 模型

## 最佳实践


- Once async always async
- Async void is BAD
- Prefer Task.FromResult over Task.Run
- Avoid .Result and .Wait
- Prefer await over ContinueWith
- Always pass the CancellationToken
- Prefer async Task over Task
- Don't sync over async in constructors
- Configure Context
  - Performance
  - Dead Lock
  - UI -> ConfigureAwait(true)
  - Library -> ConfigureAwait(false)


## 相关参考

- [What do the terms "CPU bound" and "I/O bound" mean?](https://stackoverflow.com/questions/868568/what-do-the-terms-cpu-bound-and-i-o-bound-mean)
- [Async/Await - Best Practices in Asynchronous Programming](https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)
- [Difference Between Asynchronous Programming and Multithreading in C#](https://code-maze.com/csharp-async-vs-multithreading/)
- [Asynchronous Programming with Async and Await in ASP.NET Core](https://code-maze.com/asynchronous-programming-with-async-and-await-in-asp-net-core/)
- [一个简单的模拟实例说明Task及其调度问题](https://mp.weixin.qq.com/s/mXm7oysi1E4-EsOyDxgUPA)
- [ASP.NET Core Diagnostic Scenarios](https://github.com/davidfowl/AspNetCoreDiagnosticScenarios)