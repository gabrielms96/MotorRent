#region Mainetence
/*
Comment: Created class.
Created: 08/31/2024 18:31
Author: Gabriel MS
*/
#endregion

using Amazon.CDK;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.IO;

public class S3Uploader
{
    private string bucketName = "motorrentbucket";
    private IAmazonS3 s3Client;

    public S3Uploader()
    {
        // Configuração do cliente (as credenciais devem ser configuradas)
        s3Client = new AmazonS3Client(Amazon.RegionEndpoint.USEast2);
        
    }

    public void UploadFoto(string caminhoArquivoOrigem, string nomeArquivo)
    {
        try
        {
            var fileTransferUtility = new TransferUtility(s3Client);
            var fileInfo = new FileInfo(caminhoArquivoOrigem);

            fileTransferUtility.Upload(caminhoArquivoOrigem, bucketName, nomeArquivo);

            Console.WriteLine($"Foto '{nomeArquivo}' enviada com sucesso para o bucket '{bucketName}'.");
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Erro ao fazer upload da foto: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro inesperado: {e.Message}");
        }
    }
}