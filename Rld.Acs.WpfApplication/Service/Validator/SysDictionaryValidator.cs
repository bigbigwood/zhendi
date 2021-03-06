﻿using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class SysDictionaryValidator : AbstractValidator<SysDictionary>
    {
        public SysDictionaryValidator()
        {
            RuleFor(m => m.TypeName)
                .NotEmpty().WithMessage("类型不能为空")
                .Length(1, 100).WithMessage("类型长度为1-100");

            RuleFor(m => m.LanguageID)
                .NotEmpty().WithMessage("语言ID不能为空");

            RuleFor(m => m.ItemID)
                .NotEmpty().WithMessage("数据项ID不能为空");

            RuleFor(m => m.ItemValue)
                .NotEmpty().WithMessage("数据项值不能为空")
                .Length(1, 1024).WithMessage("数据项值长度为1-1024");

            RuleFor(m => m.Remark)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.Remark)).WithMessage("备注长度为1-1024");
        }
    }
}
