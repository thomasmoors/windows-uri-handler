# windows-uri-handler
An application to handle a custom uri in Windows


This application is meant for web applications to be able to open an app on the desktop by using a custom URI.

In the example code `test://` is registered an will open the path specified after it. E.g. `test://C:\`.

You can change it by changing the [const URI](https://github.com/thomasmoors/windows-uri-handler/blob/master/ProERPUriHandler/Program.cs#L15)


The application will self-register when passing no arguments (just doubleclicking) and will handle the opening of the directory when at least 1 argument is passed. However it's easy to change the code to do something else than opening a directory.
The use case for this application is for intranet purposes.
