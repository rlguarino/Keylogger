
protoc -I .. --csharp_out . --grpc_out . --plugin=protoc-gen-grpc=..\packages\Grpc.Tools.0.7.1\tools\grpc_csharp_plugin.exe ..\ServerProxy\service.proto