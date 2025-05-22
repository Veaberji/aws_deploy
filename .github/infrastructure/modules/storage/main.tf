resource "aws_s3_bucket" "bucket" {
  bucket        = var.bucket_name
  force_destroy = true

  tags = {
    Name = "${var.app_name}-bucket"
  }

  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_s3_bucket_versioning" "bucket_versioning" {
  bucket = aws_s3_bucket.bucket.id
  versioning_configuration {
    status = var.bucket_versioning_status
  }

  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "bucket_encryption" {
  bucket = aws_s3_bucket.bucket.bucket
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = var.bucket_encryption_type
    }
  }

  lifecycle {
    prevent_destroy = true
  }
}
