variable "app_name" {
  description = "App name used for tags"
  type        = string
}

variable "bucket_name" {
  description = "Globally unique S3 bucket name"
  type        = string
}

variable "bucket_versioning_status" {
  description = "Enables bucket versioning"
  type        = string
  default     = "Enabled"
}

variable "bucket_encryption_type" {
  description = "Bucket server side encryption type"
  type        = string
  default     = "AES256"
}
