﻿namespace GpgApi
{
    internal enum GpgKeyword
    {
        None,
        NEWSIG,
        GOODSIG,
        EXPSIG,
        EXPKEYSIG,
        REVKEYSIG,
        BADSIG,
        ERRSIG,
        VALIDSIG,
        SIG_ID,
        ENC_TO,
        NODATA,
        UNEXPECTED,
        TRUST_UNDEFINED,
        TRUST_NEVER,
        TRUST_MARGINAL,
        TRUST_FULLY,
        TRUST_ULTIMATE,
        PKA_TRUST_GOOD,
        PKA_TRUST_BAD,
        SIGEXPIRED,
        KEYEXPIRED,
        KEYREVOKED,
        BADARMOR,
        RSA_OR_IDEA,
        SHM_INFO,
        SHM_GET,
        SHM_GET_BOOL,
        SHM_GET_HIDDEN,
        GET_BOOL,
        GET_LINE,
        GET_HIDDEN,
        GOT_IT,
        NEED_PASSPHRASE,
        NEED_PASSPHRASE_SYM,
        NEED_PASSPHRASE_PIN,
        MISSING_PASSPHRASE,
        BAD_PASSPHRASE,
        GOOD_PASSPHRASE,
        DECRYPTION_FAILED,
        DECRYPTION_OKAY,
        NO_PUBKEY,
        NO_SECKEY,
        IMPORT_CHECK,
        IMPORTED,
        IMPORT_OK,
        IMPORT_PROBLEM,
        IMPORT_RES,
        FILE_START,
        FILE_DONE,
        BEGIN_DECRYPTION,
        END_DECRYPTION,
        BEGIN_ENCRYPTION,
        END_ENCRYPTION,
        BEGIN_SIGNING,
        DELETE_PROBLEM,
        PROGRESS,
        SIG_CREATED,
        KEY_CREATED,
        KEY_NOT_CREATED,
        SESSION_KEY,
        NOTATION_NAME,
        NOTATION_DATA,
        USERID_HINT,
        POLICY_URL,
        BEGIN_STREAM,
        END_STREAM,
        INV_RECP,
        NO_RECP,
        ALREADY_SIGNED,
        TRUNCATED,
        ERROR,
        ATTRIBUTE,
        CARDCTRL,
        PLAINTEXT,
        PLAINTEXT_LENGTH,
        SIG_SUBPACKET,
        SC_OP_FAILURE,
        SC_OP_SUCCESS,
        BACKUP_KEY_CREATED
    }
}
