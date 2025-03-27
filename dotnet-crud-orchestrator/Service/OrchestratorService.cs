using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NetCrudStarter.Orchestrator.Dto;
using NetCrudStarter.StudentModule.Dto;
using NetCrudStarter.TeacherModule.Dto;
using Newtonsoft.Json;
using Polly; // Add Polly namespace
using Polly.Retry; // Add Polly.Retry namespace

namespace NetCrudStarter.OrchestratorModule.Service
{
    public class OrchestratorService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy; // Define retry policy

        public OrchestratorService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var baseUrl = configuration.GetValue<string>("Urls");
            _httpClient.BaseAddress = new Uri(baseUrl);

            // Initialize retry policy with Polly
            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        
        public async Task<CourseWithStudentsDto> GetCourseWithStudentsAsync(int courseId)
        {
            var courseTask = _retryPolicy.ExecuteAsync(() => _httpClient.GetStringAsync($"/api/Course/{courseId}?include-graph=WithTeacher"));
            var studentsTask = _retryPolicy.ExecuteAsync(() => _httpClient.GetStringAsync($"/api/Student?f[CourseEnroledIdEq]={courseId}"));

            await Task.WhenAll(courseTask, studentsTask);

            var course = JsonConvert.DeserializeObject<CourseDto>(courseTask.Result);
            var students = JsonConvert.DeserializeObject<List<StudentDto>>(studentsTask.Result);

            return new CourseWithStudentsDto
            {
                Course = course,
                Students = students
            };
        }
    }
}
