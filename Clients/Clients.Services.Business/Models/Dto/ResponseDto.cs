namespace Clients.Services.Business.Models.Dto
{
    public class ResponseDto<T>
    {
        public static ResponseDto<T> Error(string message)
        {
            return new ResponseDto<T>(false, message);
        }
        public static ResponseDto<T> Ok(T result)
        {
            return new ResponseDto<T>(result);
        }
        public ResponseDto()
        {

        }
        public ResponseDto(T result)
        {
            Result = result;
        }
        public ResponseDto(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public T? Result { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
