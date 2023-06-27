using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class TaskResponse
    {
        public IEnumerable<TaskModel> Data { get; set; }

        public TaskResult Result { get; set;  }
    }

    public class TaskModel
    {
        public Guid Id { get; set;  }

        public int UserId { get; set; }

        public string StaffName { get; set; }

        public TaskType Type { get; set; }

        public int LinkId { get; set; }

        public int ParentId { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedOn { get; set; }

        public TaskDateType DateType { get; set; }
        public string TypeLable { get; set; }
    }

    public class TaskSearchRequest
    {
        public int Skip { get; set; }
        public bool? IsUnread { get; set; }
        public TaskFilterType? TaskType { get; set; }
    }

    public enum TaskResult
    {
        Success = 1,
        NotFound = 2
    }

    public enum TaskDateType
    {
        Today = 1,
        Yesterday = 2,
        Older = 3
    }

    public enum TaskFilterType
    {
        Leave = 1,
        Expense = 2,
        Inspection = 3,
        Quotation = 4
    }
}
