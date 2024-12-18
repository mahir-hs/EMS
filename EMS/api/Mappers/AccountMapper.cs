using api.Dto.Account;
using api.Models;

namespace api.Mappers
{
    public static class AccountMapper
    {
        public static AccountDto  ToAccountDto(this Account? account)
        {
            return account == null ? throw new ArgumentNullException(nameof(account)) :
            new AccountDto
            {
                Id = account.Id,
                Email = account.Email,
                Password = account.Password,
                RoleId = account.RoleId,
                RoleName = account.RoleName,
                Token = account.Token,
                RefreshToken = account.RefreshToken,
                RefreshTokenExpiryTime = account.RefreshTokenExpiryTime

            };
        }

        public static Account ToAccount(this AccountUpdateDto dto, Account account)
        {
            if (!string.IsNullOrEmpty(dto.Email)) account.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password)) account.Password = dto.Password;
            if (dto.RoleId.HasValue) account.RoleId = dto.RoleId.Value;
            if (!string.IsNullOrEmpty(dto.RoleName)) account.RoleName = dto.RoleName;
            if (!string.IsNullOrEmpty(dto.Token)) account.Token = dto.Token;
            if (!string.IsNullOrEmpty(dto.RefreshToken)) account.RefreshToken = dto.RefreshToken;
            if (dto.RefreshTokenExpiryTime.HasValue) account.RefreshTokenExpiryTime = dto.RefreshTokenExpiryTime.Value;
            return account;
        }
    }
}
