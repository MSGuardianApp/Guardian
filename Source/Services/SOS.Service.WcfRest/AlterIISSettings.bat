%windir%\system32\inetsrv\appcmd set config -section:system.web/processModel /autoConfig:"false" /minWorkerThreads:"100" /maxWorkerThreads:"1000" /maxIoThreads:"1000" /minIoThreads:"100" /commit:machine

%windir%\system32\inetsrv\appcmd set config -section:applicationPools -applicationPoolDefaults.processModel.idleTimeout:00:00:00