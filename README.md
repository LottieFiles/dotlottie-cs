# dotlottie-cs

This package enables functionality for the dotlottie format inside netstandard compatible applications.

The package is available at: [https://www.nuget.org/packages/LottieFiles.DotLottie](https://www.nuget.org/packages/LottieFiles.DotLottie)


#### Usage
1. Install via package manager console or via nuget manager:
    ``` dotnet add package LottieFiles.DotLottie ```

2. Use
    ``` 
    var stream = File.OpenRead("example.lottie");
    DotLottie lottie = await DotLottie.OpenAsync(stream);
    var animation = lottie.FirstAnimation();
    var animationName = animation.Key;
    var animationJson = animation.Value;
    ```
#### Development
You can develop with your IDE of choice on Windows, macOS or Linux, typically Visual Studio, Jetbrains Rider or VS Code are good options. The project targets dotnet standard 2.0, and should work with NetFramework, NetCore and UWP.

We welcome feedback and contributions.

#### Documentation
Full documentation is available [here](https://app.gitbook.com/@lottiefiles/s/dotlottie)

#### dotLottie Project Homepage
The dotLottie project site is [https://dotlottie.io/intro/](https://dotlottie.io/intro/)




