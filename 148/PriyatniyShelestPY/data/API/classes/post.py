from pydantic import BaseModel, validator

class agent(BaseModel):
    ag_id: int = 0
    ag_title: str = "title"
    ag_type: str = "type"
    ag_address: str = "address"
    ag_director: str = "director"
    ag_email: str = "email"
    ag_phone: str = "01234567890"
    ag_inn: str = "01234567890"
    ag_kpp: str = "012345678"

    ag_sales: int = 0
    ag_disc: int = 0
    img: bytes = ""

    @validator("ag_phone")
    def phone_length(cls, v):
        if len(v) != 11:
            raise ValueError("Номер телефона должен быть из 11 цифр")

    @validator("ag_inn")
    def inn_length(cls, v):
        if len(v) != 11:
            raise ValueError("ИНН должен быть из 11 цифр")
        return v

    @validator("ag_kpp")
    def kpp_length(cls, v):
        if len(v) != 9:
            raise ValueError("КПП должен быть из 9 цифр")
        return v