using JobBoard.Domain.Enums;

namespace JobBoard.Application.DTO
{
    public class Account
    {
        public int Id { get; set; }
        public EnumAccountType AccountType { get; set; }
    }
}
