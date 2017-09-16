
# SaidOut.Security.SecureRandom [![NuGet Version](https://img.shields.io/nuget/v/SaidOut.Security.SecureRandom.svg?style=flat)](https://www.nuget.org/packages/SaidOut.Security.SecureRandom/)
Contain classes that generate random data/value using a cryptographic random number generator.

---
## Table of Content
 * [Classes](#classes)
   * [SaidOut.Security.ISecureRandom](#isecurerandom)
   * [SaidOut.Security.SecureRandom](#securerandom)
   * [SaidOut.Security.SecureRandomContext](#securerandomcontext)

---
## Classes
 * [SaidOut.Security.ISecureRandom](#isecurerandom) Generate random data/value using a cryptographic random number generator.
 * [SaidOut.Security.SecureRandom](#securerandom) Generate random data/value using a new cryptographic random number generator each time it generates a random data/value.
 * [SaidOut.Security.SecureRandomContext](#securerandomcontext) Generate random data/value using the same cryptographic random number generator each time it generates a random data/value in the same random context.



 ---
### ISecureRandom
| Name | Description |
|--------|-------------|
| `GenerateRandomData` | Create a byte array of `size` with random data. |
| `GenerateRandomValue` | Generate a random value in the range [`min`, `max`]. |

Interface is implemented by `SecureRandom` and `SecureRandomContext`.

---
### SecureRandom

| Name | Description |
|--------|-------------|
| `GenerateRandomData` | Create a byte array of `size` with random data. |
| `GenerateRandomValue` | Generate a random value in the range [`min`, `max`]. |

Generate random data/value using a new cryptographic random number generator each time it generates a random data/value.  
If you need to generate multiple random data/values at the same time use `SecureRandomContext`.  

Example
```cs
    var nonce = SecureRandom.GenerateRandomData(15);  
    var radomVal = SecureRandom.GenerateRandomValue(10, 50);  // radomVal  set to a value between 10 and 50.  
```


---
### SecureRandomContext

| Name | Description |
|--------|-------------|
| `GenerateRandomData` | Create a byte array of `size` with random data. |
| `GenerateRandomValue` | Generate a random value in the range [`min`, `max`]. |

Generate random data/value using the same cryptographic random number generator each time it generates a random data/value in the same random context.  
Use this class if you need to generate multiple random data/values at the same time use.  

Example
```cs
    using (var secRndCtx = new SecureRandomContext())  
    {  
        var keyA = secRndCtx.GenerateRandomData(15);  
        var keyB = secRndCtx.GenerateRandomData(15);  
        var nonce = secRndCtx.GenerateRandomData(15);  

        var randomVal = secRndCtx.GenerateRandomValue(10, 50);  
    }  
```
